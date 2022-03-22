using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql.MySql
{
    internal class DataUpdater
    {
        private readonly string _connectionString;

        public DataUpdater(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task RunInsertScripts(InsertScripts insertScripts)
        {
            using var connection = new MySqlConnection(_connectionString);
            foreach (var insertScript in insertScripts.Scripts)
            {
                var dynamicParameters = new DynamicParameters(insertScript.Parameters);
                int result = await connection.ExecuteAsync(insertScript.Script, dynamicParameters);
            }
        }

    }
}
