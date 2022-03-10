using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core
{
    public class ValueGenerator : IValueGenerator
    {
        private readonly bool _isRandom;
        private readonly Random _random = new Random();
        private readonly DateTime _functionalDate;
        private readonly DateTime _earliestDate;
        private readonly DateTime _latestDate;

        public ValueGenerator(bool isRandom, DateTime functionalDate,
            DateTime earliestDate, DateTime latestDate)
        {
            _isRandom = isRandom;
            _functionalDate = functionalDate;
            _earliestDate = earliestDate;
            _latestDate = latestDate;
        }

        public object? GenerateValue(string columnName, string dataType, bool isNullable, long maxLength)
        {
            switch (dataType)
            {
                case "char":
                case "varchar":
                case "longtext":
                    return GenerateValueString(columnName, isNullable, maxLength);

                case "date":
                case "datetime":
                case "timestamp":
                    return GenerateValueDate(columnName, isNullable);
                    
                case "int":
                    return int.MaxValue;

                case "tinyint":
                    return 0;

                case "decimal":
                    return 0;

                default:
                    throw new Exception($"Data type not available {dataType}");
            }
            
        }

        private DateTime? GenerateValueDate(string columnName, bool isNullable)
        {
            if (!_isRandom) DateTime.MaxValue.AddDays(-1);

            if (isNullable && _random.Next(10) == 1) return null;

            if (columnName.Contains("updated", StringComparison.OrdinalIgnoreCase))
            {
                return _functionalDate;
            }

            return RandomDate(_earliestDate, _latestDate);
        }

        private string? GenerateValueString(string columnName, bool isNullable, long stringLength)
        {
            if (!_isRandom) return "1";

            if (isNullable && _random.Next(10) == 1) return null;

            if (columnName.Contains("firstname", StringComparison.OrdinalIgnoreCase))
            {
                return FirstNames[_random.Next(FirstNames.Length)];
            }
            else if (columnName.Contains("lastname", StringComparison.OrdinalIgnoreCase)
                || columnName.Contains("surname", StringComparison.OrdinalIgnoreCase))
            {
                return LastNames[_random.Next(LastNames.Length)];
            }
            else if (columnName.Contains("name", StringComparison.OrdinalIgnoreCase))
            {
                return FirstNames[_random.Next(FirstNames.Length)];
            }

            return RandomString(stringLength);
        }

        // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        public string RandomString(long length)
        {
            int repeat = (length > 1000) ? 1000 : (int)length;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, repeat)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        private DateTime? RandomDate(DateTime start, DateTime end)
        {
            int days = (end - start).Days;
            return start.AddDays(_random.Next(days));
        }

        private string[] FirstNames = new[]
        {
            "Albert", "Abigail", "Amy", "Arnold", "Allan", "Ahmed", "Brian", "Billy", 
            "Barbara", "Bridget", "Carl", "Ceri",
            "Catherine", "Charlotte", "Dennis", "Donald", "Diedre", "Ian"
        };

        private string[] LastNames = new[]
        {
            "Briggs", "Bennet", "Crowther", "Corbett", "Dodds", "Einstein", 
            "Heisenburg", "O'Shea", "Van-Halen", "Kilmister",
            "Zuckowski"
        };

        public int? IntGeneric(bool allowNulls)
        {
            if (!_isRandom) return int.MaxValue;

            if (allowNulls)
            {
                if (_random.Next(2) == 1) return null;
            }
            return _random.Next(int.MaxValue);
        }

        public int? Int(string fieldName, bool allowNulls)
        {
            throw new NotImplementedException();
        }
    }
}
