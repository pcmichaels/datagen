using Castr;
using Castr.Class;
using Dapper;
using datagen.Core.DataAccessor;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql.MetaData
{
    internal class GetMetaData : IGetMetaData
    {
        private readonly string _connectionString;


        public GetMetaData(string connectionString)
        {
            _connectionString = connectionString;
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

        public IEnumerable<Dictionary<string, object>> GetColumnDefinitions(Dictionary<string, string> parameters)
        {
            string tableName = parameters["TableName"];
            string schema = parameters["Schema"];

            using var connection = new MySqlConnection(_connectionString);
            var dataDefinition = connection.Query<DataDefinition>(
                "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, " +
                "COLUMN_KEY, EXTRA " +
                "FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = @tableName " +
                "AND TABLE_SCHEMA = @schema",
               new { tableName, schema });

            var returnDictionaryList = new List<Dictionary<string, object>>();

            foreach (var data in dataDefinition)
            {
                var castr = new CastrClass<DataDefinition>(data);
                var dictionary = castr.CastAsDictionary();
                returnDictionaryList.Add(dictionary);
            }
            return returnDictionaryList;
        }

        public IEnumerable<Dictionary<string, object>> GetColumnData(Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
