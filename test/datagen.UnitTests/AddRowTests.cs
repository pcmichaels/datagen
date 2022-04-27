using datagen.Core;
using datagen.Core.DataAccessor;
using datagen.MySql;
using datagen.MySql.MySql;
using Moq;
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

            var dataAccessorFactory = new Mock<IDataAccessorFactory>();
            
            dataAccessorFactory.Setup(a => a.GetDataReader<IDataAccessorFactory>(
                It.IsAny<Func<IDataAccessorFactory>>()))
                .Returns(dataAccessorFactory.Object);

            var mySqlDefaults = new MySqlDefaults("");            
            var generate = new Generate("", 
                valueGenerator, 
                mySqlDefaults.DataTypeParser,
                mySqlDefaults.UniqueKeyGenerator,
                dataAccessorFactory.Object);

            // Act
            await generate.AddRow("some_table", 2, "my_schema");

            // Assert


        }
    }
}
