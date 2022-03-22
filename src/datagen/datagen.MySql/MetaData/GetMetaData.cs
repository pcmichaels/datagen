using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql.MetaData
{
    internal class GetMetaData
    {
        private readonly string _connectionString;

        public GetMetaData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<DataDefinition> GetColumnDefinitions(string tableName, string schema)
        {
            using var connection = new MySqlConnection(_connectionString);
            var dataDefinition = connection.Query<DataDefinition>(
                "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, " +
                "COLUMN_KEY, EXTRA " +
                "FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = @tableName " +
                "AND TABLE_SCHEMA = @schema",
               new { tableName, schema });
            return dataDefinition;
        }

        public IEnumerable<ColumnKeys> GetColumnData(string tableName, string schema)
        {
            using var connection = new MySqlConnection(_connectionString);
            var foreignKeys = connection.Query<ColumnKeys>(
                "SELECT CONSTRAINT_NAME, COLUMN_NAME, " +
                "REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "WHERE TABLE_NAME = @tableName " +
                "AND CONSTRAINT_SCHEMA = @schema",
               new { tableName, schema });
            return foreignKeys;
        }
    }
}
