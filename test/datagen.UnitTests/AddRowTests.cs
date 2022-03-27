using datagen.Core;
using datagen.Core.DataAccessor;
using datagen.MySql;
using datagen.MySql.MySql;
using datagen.UnitTests.Mocks;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace datagen.UnitTests
{
    public class AddRowTests
    {
        [Fact]
        public async Task AddRow_NullParameters_NothingAdded()
        {
            // Arrange
            var valueGenerator = new ValueGenerator(
                false,
                DateTime.Now,
                DateTime.Now.AddDays(-100),
                DateTime.Now.AddDays(10));

            var dataAccessorFactory = Substitute.For<IDataAccessorFactory>();
            var reader = dataAccessorFactory.GetDataReader<>(() => new GetMetaDataTest());

            var mySqlDefaults = new MySqlDefaults("");            
            var generate = new Generate("", 
                valueGenerator, 
                mySqlDefaults.DataTypeParser,
                mySqlDefaults.UniqueKeyGenerator,
                dataAccessorFactory);

            // Act
            await generate.AddRow("some_table", 2, "my_schema");

            // Assert


        }
    }
}
