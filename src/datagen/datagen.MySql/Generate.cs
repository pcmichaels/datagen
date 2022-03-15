using MySql.Data.MySqlClient;
using Dapper;
using datagen.Core;
using datagen.MySql.MetaData;

namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;
        private readonly IValueGenerator _valueGenerator;
        private readonly IDataTypeParser _dataTypeParser;

        public Generate(string connectionString, IValueGenerator valueGenerator,
            IDataTypeParser dataTypeParser)
        {
            _connectionString = connectionString;
            _valueGenerator = valueGenerator;
            _dataTypeParser = dataTypeParser;
        }

        public async Task AddRow(string tableName, int count, string schema)
        {
            using var connection = new MySqlConnection(_connectionString);
            var dataDefinition = connection.Query<DataDefinition>(
                "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, " +
                "COLUMN_KEY, EXTRA " +
                "FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = @tableName " +
                "AND TABLE_SCHEMA = @tableSchema",
               new { tableName, schema });

            // Check for foreign keys
            var foreignKeys = connection.Query<ColumnKeys>(
                "SELECT CONSTRAINT_NAME, COLUMN_NAME, " +
                "REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "WHERE TABLE_NAME = @tableName " + 
                "AND CONSTRAINT_SCHEMA = @schema",
               new { tableName, schema });

            for (int i = 0; i < count; i++)
            {
                var insertScript = GenerateInsertScript(
                    dataDefinition, foreignKeys, tableName);

                var dynamicParameters = new DynamicParameters(insertScript.Parameters);
                int result = await connection.ExecuteAsync(insertScript.Script, dynamicParameters);
            }
        }

        private InsertScript GenerateInsertScript(
            IEnumerable<DataDefinition> dataDefinitions,
            IEnumerable<ColumnKeys> columnKeys,
            string tableName)
        {
            string fields = string.Empty;
            string values = string.Empty;
            InsertScript insertScript = new InsertScript();

            foreach (var dataDefinition in dataDefinitions)
            {
                if (dataDefinition.Extra == MySqlConstants.KEY_AUTO_INCREMENT) continue;
                if (columnKeys.Any(a => a.Column_Name == dataDefinition.Column_Name
                    && a.Constraint_Name != MySqlConstants.CONSTRAINT_PRIMARY)) continue;

                if (!string.IsNullOrWhiteSpace(fields)) fields += ",";
                fields += $"`{dataDefinition.Column_Name}`";
                if (!string.IsNullOrWhiteSpace(values)) values += ",";
                values += $"@{dataDefinition.Column_Name}Value";

                insertScript.Parameters.Add(dataDefinition.Column_Name, dataDefinition.Column_Name);

                if (dataDefinition.Column_Key == MySqlConstants.KEY_PRIMARY)
                {
                    var key = GenerateUniqueKey(
                        tableName, 
                        dataDefinition.Column_Name, 
                        dataDefinition.Character_Maximum_Length ?? 0, 
                        dataDefinition.Data_Type);
                    insertScript.Parameters.Add(
                        $"{dataDefinition.Column_Name}Value", key);
                }
                else
                {
                    insertScript.Parameters.Add(
                        $"{dataDefinition.Column_Name}Value",
                        _valueGenerator.GenerateValue(dataDefinition.Column_Name,
                            dataDefinition.Data_Type, dataDefinition.Is_Nullable,
                            dataDefinition.Character_Maximum_Length ?? 0));
                }
            }

            insertScript.Script = $"INSERT INTO {tableName} ({fields}) VALUES ({values})";
            //insertScript.Parameters.Add("tableName", tableName);

            return insertScript;
        }

        // ToDo: Separate into UniqueKeyGenerator class
        private object GenerateUniqueKey(string tableName, string columnName, long columnLength, string dataType)
        {            
            if (_dataTypeParser.IsTypeInteger(dataType))
            {
                using var connection = new MySqlConnection(_connectionString);

                string sql = $"SELECT MAX(`{columnName}`) " +
                    $"FROM {tableName}";
                int pk = connection.ExecuteScalar<int>(sql);                    
                return pk + 1;
            }
            else if  (_dataTypeParser.IsTypeString(dataType))
            {
                return Truncate(Guid.NewGuid().ToString(), (int)columnLength);                
            }
            throw new Exception($"Unable to process primary key of type {dataType}");
        }

        // https://stackoverflow.com/questions/2776673/how-do-i-truncate-a-net-string
        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public async Task FillSchema(int rowsPerTable, string schema)
        {
            IEnumerable<string> tables;
            string sql = "SELECT table_name FROM information_schema.tables " +
                "WHERE TABLE_SCHEMA = @tableSchema";
            using (var connection = new MySqlConnection(_connectionString))
            {
                tables = connection.Query<string>(sql, new {schema});
            }

            foreach (var table in tables)
            {
                await AddRow(table, rowsPerTable, schema);
            }
        }
    }
}