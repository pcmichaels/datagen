using Dapper;
using datagen.Core;
using MySql.Data.MySqlClient;

namespace datagen.MySql.KeyGeneration
{
    public class UniqueKeyGenerator : IUniqueKeyGenerator
    {
        private readonly string _connectionString;
        private readonly IDataTypeParser _dataTypeParser;        

        public UniqueKeyGenerator(
            string connectionString,
            IDataTypeParser dataTypeParser)
        {
            _connectionString = connectionString;
            _dataTypeParser = dataTypeParser;            
        }

        public object GenerateUniqueKey(string tableName, string columnName, long columnLength, string dataType)
        {
            if (_dataTypeParser.IsTypeInteger(dataType))
            {
                using var connection = new MySqlConnection(_connectionString);

                string sql = $"SELECT MAX(`{columnName}`) " +
                    $"FROM {tableName}";
                int pk = connection.ExecuteScalar<int>(sql);
                return pk + 1;
            }
            else if (_dataTypeParser.IsTypeString(dataType))
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

    }
}
