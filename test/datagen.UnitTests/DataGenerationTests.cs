using datagen.Core;
using Xunit;

namespace datagen.UnitTests
{
    record DateParameters(    
        DateTime FunctionalDate,
        DateTime EarliestDate,
        DateTime LatestDate);

    public class DataGenerationTests
    {
        private DateParameters AssignDateValues() =>
            new DateParameters(
                new DateTime(2022, 01, 01),
                new DateTime(2000, 01, 01),
                new DateTime(2025, 01, 01));        

        [Fact]
        public void ValueGenerator_int_NonSpecific()
        {
            // Arrange
            var dateParameters = AssignDateValues();
            var valueGenerator = new ValueGenerator(false, dateParameters.FunctionalDate,
                dateParameters.EarliestDate, dateParameters.LatestDate);

            // Act
            int? value = valueGenerator.IntGeneric(true);

            // Assert
            Assert.True(value.HasValue);
            Assert.Equal(int.MaxValue, value!.Value);
        }

        [Fact]
        public void ValueGenerator_GenerateValue_int_Returns1()
        {
            // Arrange
            var dateParameters = AssignDateValues();
            var valueGenerator = new ValueGenerator(false, dateParameters.FunctionalDate,
                dateParameters.EarliestDate, dateParameters.LatestDate);

            // Act
            var result = valueGenerator.GenerateValue("number", "int", false, 20);

            // Assert
            Assert.Equal(int.MaxValue, result);
        }
    }
}
