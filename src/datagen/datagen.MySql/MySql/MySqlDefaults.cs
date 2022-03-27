using datagen.Core;
using datagen.Core.DataAccessor;
using datagen.MySql.KeyGeneration;

namespace datagen.MySql.MySql
{
    public class MySqlDefaults
    {       
        private readonly IDataTypeParser _dataTypeParser;
        private readonly IUniqueKeyGenerator _uniqueKeyGenerator;
        private readonly IDataAccessorFactory _dataAccessorFactory;

        public MySqlDefaults(string connectionString)
        {            
            _dataTypeParser = new MySqlDataTypeParser();
            _uniqueKeyGenerator = new UniqueKeyGenerator(connectionString, _dataTypeParser);
            _dataAccessorFactory = new DataAccessorFactory();
        }

        public IDataTypeParser DataTypeParser
        {
            get => _dataTypeParser; 
        }

        public IUniqueKeyGenerator UniqueKeyGenerator
        {
            get => _uniqueKeyGenerator;
        }

        public IDataAccessorFactory DataAccessorFactory
        {
            get => _dataAccessorFactory;
        }
    }
}
