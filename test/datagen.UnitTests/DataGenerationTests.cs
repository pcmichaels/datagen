using datagen.Core;
using Xunit;

namespace datagen.UnitTests
{
    public class DataGenerationTests
    {
        [Fact]
        public void ValueGenerator_int_NonSpecific()
        {
            // Arrange
            var valueGenerator = new ValueGenerator(false);

            // Act
            int? value = valueGenerator.IntGeneric(true);

            // Assert
            Assert.Equal(int.MaxValue, value.Value);
        }

        [Fact]
        public void ValueGenerator_GenerateValue_int_Returns1()
        {
            // Arrange
            var valueGenerator = new ValueGenerator(false);

            // Act
            var result = valueGenerator.GenerateValue("number", "int", false);

            // Assert
            Assert.Equal(1, result);
        }
    }
}
