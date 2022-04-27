using MySql.Data.MySqlClient;
using Dapper;
using datagen.Core;
using datagen.MySql.MetaData;
using datagen.MySql.MySql;
using datagen.Core.DataAccessor;

namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;
        private readonly IValueGenerator _valueGenerator;
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IUniqueKeyGenerator _uniqueKeyGenerator;
        private readonly IDataAccessorFactory _dataAccessorFactory;        
        private Dictionary<string, string> _foreignKeyMappings = new Dictionary<string, string>();

        public Generate(    
            string connectionString,
            IValueGenerator valueGenerator,
            IDataTypeParser dataTypeParser,
            IUniqueKeyGenerator uniqueKeyGenerator,
            IDataAccessorFactory dataAccessorFactory)
        {
            _connectionString = connectionString;
            _valueGenerator = valueGenerator;
            _dataTypeParser = dataTypeParser;
            _uniqueKeyGenerator = uniqueKeyGenerator;
            _dataAccessorFactory = dataAccessorFactory;
        }

        public void CreateForeignKeyMapping(string from, string to) =>        
            _foreignKeyMappings.Add(from, to);        

        public async Task AddRow(string tableName, int count, string schema, object? primaryKey = null)
        {
            if (count > 1 && primaryKey != null)            
                throw new Exception("Primary Key can only be specified for a single row update");
            
            if (count < 0)            
                throw new Exception("Count must be greater than zero");

            var getMetaData = _dataAccessorFactory.GetDataReader<GetMetaData>(() => new GetMetaData(_connectionString));
            if (getMetaData == null) return;

            var dataDefinitionDict = getMetaData.GetColumnDefinitions(
                new Dictionary<string, string>() { 
                    { "TableName", tableName },
                    { "Schema", schema } 
                });
            var dataDefinitionList = new List<DataDefinition>();

            foreach (var dataDefinitionEntry in dataDefinitionDict)
            {
                var dataDefinition = DataDefinition.ConvertFromDict(
                    dataDefinitionEntry);
                dataDefinitionList.Add(dataDefinition);
            }

            var foreignKeys = getMetaData.GetColumnData(tableName, schema);
            
            var dataUpdater = _dataAccessorFactory.GetDataUpdater<DataUpdater>(() => new DataUpdater(_connectionString));
            if (dataUpdater == null) return;

            for (int i = 0; i < count; i++)
            {
                var insertScripts = await GenerateInsertScript(
                    dataDefinitionList, 
                    foreignKeys, tableName, schema, primaryKey);

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

                var mappings = _foreignKeyMappings.Where(a => a.Key == $"{tableName}.{dataDefinition.Column_Name}");

                if (mappings.Any())
                {
                    var mappingDestination = mappings.First().Value.Split('.');
                    if (columnKey == null)
                        columnKey = new ColumnKeys();
                    columnKey.Referenced_Table_Name = mappingDestination[0];
                    columnKey.Referenced_Column_Name = mappingDestination[1];
                    var value = await AddForeignKey(schema, insertScript, dataDefinition, columnKey);
                    AddFieldValues(ref fields, ref values, dataDefinition);
                    insertScript.Parameters.Add(dataDefinition.Column_Name, dataDefinition.Column_Name);
                    continue;
                }

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