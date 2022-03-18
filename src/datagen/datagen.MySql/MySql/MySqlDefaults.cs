using datagen.Core;
using datagen.MySql.KeyGeneration;

namespace datagen.MySql.MySql
{
    public class MySqlDefaults
    {       
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IUniqueKeyGenerator _uniqueKeyGenerator;

        public MySqlDefaults(string connectionString)
        {            
            _dataTypeParser = new MySqlDataTypeParser();
            _uniqueKeyGenerator = new UniqueKeyGenerator(connectionString, _dataTypeParser);
        }

        public IDataTypeParser DataTypeParser
        {
            get => _dataTypeParser; 
        }

        public IUniqueKeyGenerator UniqueKeyGenerator
        {
            get => _uniqueKeyGenerator;
        }
    }
}
