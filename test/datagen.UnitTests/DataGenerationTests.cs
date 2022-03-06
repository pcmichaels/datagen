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
            Assert.Equal(0, value.Value);
        }
    }
}