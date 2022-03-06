﻿using MySql.Data.MySqlClient;
using Dapper;
using datagen.Core;

namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;
        private readonly IValueGenerator _valueGenerator;

        public Generate(string connectionString, IValueGenerator valueGenerator)
        {
            _connectionString = connectionString;
            _valueGenerator = valueGenerator;
        }

        public async Task AddRow(string tableName)
        {
            using var connection = new MySqlConnection(_connectionString);
            var dataDefinition = connection.Query<DataDefinition>(
                "SELECT column_name, data_type, character_maximum_length " +
                "FROM information_schema.columns " +
                "WHERE table_name = @tableName",
               new { tableName });

            var insertScript = GenerateInsertScript(dataDefinition, tableName);

            var dynamicParameters = new DynamicParameters(insertScript.Parameters);            
            int result = await connection.ExecuteAsync(insertScript.Script, dynamicParameters);
        }

        private InsertScript GenerateInsertScript(
            IEnumerable<DataDefinition> dataDefinitions,
            string tableName)
        {
            string fields = string.Empty;
            string values = string.Empty;
            InsertScript insertScript = new InsertScript();

            foreach (var dataDefinition in dataDefinitions)
            {
                if (!string.IsNullOrWhiteSpace(fields)) fields += ",";
                fields += $"`{dataDefinition.Column_Name}`";
                if (!string.IsNullOrWhiteSpace(values)) values += ",";
                values += $"@{dataDefinition.Column_Name}Value";

                insertScript.Parameters.Add(dataDefinition.Column_Name, dataDefinition.Column_Name);
                insertScript.Parameters.Add(
                    $"{dataDefinition.Column_Name}Value",
                    _valueGenerator.GenerateValue(dataDefinition.Column_Name, 
                        dataDefinition.Data_Type, dataDefinition.Is_Nullable));
            }

            insertScript.Script = $"INSERT INTO {tableName} ({fields}) VALUES ({values})";
            //insertScript.Parameters.Add("tableName", tableName);

            return insertScript;
        }

        public void FillColumn()
        {
            throw new NotImplementedException();
        }

        public void FillDB()
        {
            throw new NotImplementedException();
        }

        public void FillTable()
        {
            throw new NotImplementedException();
        }
    }
}