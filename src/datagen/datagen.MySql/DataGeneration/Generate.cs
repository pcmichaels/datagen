using MySql.Data.MySqlClient;
using Dapper;
using datagen.Core;
using datagen.MySql.MetaData;
using datagen.MySql.MySql;

namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;
        private readonly IValueGenerator _valueGenerator;
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IUniqueKeyGenerator _uniqueKeyGenerator;       

        public Generate(    
            string connectionString,
            IValueGenerator valueGenerator,
            IDataTypeParser dataTypeParser,
            IUniqueKeyGenerator uniqueKeyGenerator)
        {
            _connectionString = connectionString;
            _valueGenerator = valueGenerator;
            _dataTypeParser = dataTypeParser;
            _uniqueKeyGenerator = uniqueKeyGenerator;            
        }

        public async Task AddRow(string tableName, int count, string schema, object primaryKey = null)
        {
            var getMetaData = new GetMetaData(_connectionString);
            var dataDefinition = getMetaData.GetColumnDefinitions(tableName, schema);
            var foreignKeys = getMetaData.GetColumnData(tableName, schema);

            // ToDo - refactor
            var dataUpdater = new DataUpdater(_connectionString);

            for (int i = 0; i < count; i++)
            {
                var insertScripts = await GenerateInsertScript(
                    dataDefinition, foreignKeys, tableName, schema, primaryKey);

                // ToDo - refactor
                await dataUpdater.RunInsertScripts(insertScripts);
            }
        }

        private async Task<InsertScripts> GenerateInsertScript(
            IEnumerable<DataDefinition> dataDefinitions,
            IEnumerable<ColumnKeys> columnKeys,
            string tableName, 
            string schema,
            object? primaryKey = null)
        {
            string fields = string.Empty;
            string values = string.Empty;
            InsertScripts insertScripts = new InsertScripts();
            var insertScript = insertScripts.AddLast();

            foreach (var dataDefinition in dataDefinitions)
            {                
                if (dataDefinition.Extra == MySqlConstants.KEY_AUTO_INCREMENT) continue;

                var columnKey = columnKeys.FirstOrDefault(a => a.Column_Name == dataDefinition.Column_Name
                    && a.Constraint_Name != MySqlConstants.CONSTRAINT_PRIMARY);
                if (columnKey != null) // Foreign Key
                {
                    var value = await AddForeignKey(schema, insertScript, dataDefinition, columnKey);
                    AddFieldValues(ref fields, ref values, dataDefinition);
                    insertScript.Parameters.Add(dataDefinition.Column_Name, dataDefinition.Column_Name);

                    continue;
                }
                else
                {
                    AddFieldValues(ref fields, ref values, dataDefinition);
                    insertScript.Parameters.Add(dataDefinition.Column_Name, dataDefinition.Column_Name);

                    if (dataDefinition.Column_Key == MySqlConstants.KEY_PRIMARY)
                    {
                        primaryKey = AddPrimaryKey(tableName, primaryKey, insertScript, dataDefinition);
                    }
                    else
                    {
                        var fieldValue = AddField(insertScript, dataDefinition);
                    }
                }
            }

            insertScript.Script = $"INSERT INTO {tableName} ({fields}) VALUES ({values})";
            return insertScripts;
        }

        private object? AddField(InsertScript insertScript, DataDefinition dataDefinition)
        {
            var value = _valueGenerator.GenerateValue(dataDefinition.Column_Name,
                    dataDefinition.Data_Type, dataDefinition.Is_Nullable,
                    dataDefinition.Character_Maximum_Length ?? 0);

            insertScript.Parameters.Add(
                $"{dataDefinition.Column_Name}Value",
                value!);
            return value;
        }

        private async Task<object?> AddForeignKey(string schema, InsertScript insertScript, DataDefinition dataDefinition, ColumnKeys? columnKey)
        {
            ArgumentNullException.ThrowIfNull(columnKey);

            var value = _uniqueKeyGenerator.GenerateUniqueKey(
                columnKey.Referenced_Table_Name,
                columnKey.Referenced_Column_Name,
                dataDefinition.Character_Maximum_Length ?? 0,
                dataDefinition.Data_Type);

            await AddRow(columnKey.Referenced_Table_Name, 1, schema, value!);
            insertScript.Parameters.Add(
                $"{dataDefinition.Column_Name}Value", value!);
            return value;
        }

        private object AddPrimaryKey(string tableName, object? primaryKey, InsertScript insertScript, DataDefinition dataDefinition)
        {
            if (primaryKey == null)
            {
                primaryKey = _uniqueKeyGenerator.GenerateUniqueKey(
                    tableName,
                    dataDefinition.Column_Name,
                    dataDefinition.Character_Maximum_Length ?? 0,
                    dataDefinition.Data_Type);
            }
            insertScript.Parameters.Add(
                $"{dataDefinition.Column_Name}Value", primaryKey);
            return primaryKey;
        }

        private static void AddFieldValues(ref string fields, ref string values, DataDefinition dataDefinition)
        {
            if (!string.IsNullOrWhiteSpace(fields)) fields += ",";
            fields += $"`{dataDefinition.Column_Name}`";
            if (!string.IsNullOrWhiteSpace(values)) values += ",";
            values += $"@{dataDefinition.Column_Name}Value";
        }

        public async Task FillSchema(int rowsPerTable, string schema)
        {            
            string sql = "SELECT table_name FROM information_schema.tables " +
                "WHERE TABLE_SCHEMA = @schema";
            using var connection = new MySqlConnection(_connectionString);
            var tables = connection.Query<string>(sql, new { schema });            

            foreach (var table in tables)
            {
                await AddRow(table, rowsPerTable, schema);
            }
        }
    }
}