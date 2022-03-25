using datagen.Core;
using datagen.MySql;
using datagen.MySql.MySql;
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

            var mySqlDefaults = new MySqlDefaults("");
            var generate = new Generate("", 
                valueGenerator, 
                mySqlDefaults.DataTypeParser,
                mySqlDefaults.UniqueKeyGenerator);

            // Act
            await generate.AddRow("some_table", 2, "my_schema");

            // Assert


        }
    }
}
