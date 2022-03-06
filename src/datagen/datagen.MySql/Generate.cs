using MySql.Data.MySqlClient;
using Dapper;

namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;

        public Generate(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddRow(string tableName)
        {
            using var connection = new MySqlConnection(_connectionString);
            var result = connection.Query<string>(
                "SELECT column_name " +
                "FROM information_schema.columns " +
                "WHERE table_name = @tableName",
               new { tableName });
            

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