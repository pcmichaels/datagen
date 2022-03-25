using datagen.Core.Extensions;
using datagen.Core.PseudoData;
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
                    return GenerateValueNumber(int.MaxValue);

                case "tinyint":
                    return GenerateValueNumber(256);

                case "decimal":
                    return GenerateValueFloat(columnName);

                default:
                    throw new Exception($"Data type not available {dataType}");
            }
            
        }
        
        private decimal? GenerateValueFloat(string columnName, decimal maxValue = decimal.MaxValue)
        {
            if (!_isRandom) return maxValue;

            if (columnName.Contains("balance", StringComparison.OrdinalIgnoreCase))
            {
                return _random.NextDecimal(Numbers.MONEY_MAX, Numbers.MONEY_MIN);
            }

            return _random.NextDecimal();
        }

        private int? GenerateValueNumber(int maxValue)
        {
            if (!_isRandom) return maxValue;

            return _random.Next(maxValue);
        }

        private DateTime? GenerateValueDate(string columnName, bool isNullable)
        {
            if (!_isRandom) return _latestDate;

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
                return Names.FirstNames[_random.Next(Names.FirstNames.Length)];
            }
            else if (columnName.Contains("lastname", StringComparison.OrdinalIgnoreCase)
                || columnName.Contains("surname", StringComparison.OrdinalIgnoreCase))
            {
                return Names.LastNames[_random.Next(Names.LastNames.Length)];
            }
            else if (columnName.Contains("name", StringComparison.OrdinalIgnoreCase))
            {
                return Names.FirstNames[_random.Next(Names.FirstNames.Length)];
            }
            else if (columnName.Contains("email", StringComparison.OrdinalIgnoreCase))
            {
                return $"{Names.FirstNames[_random.Next(Names.FirstNames.Length)]}@{RandomString(5, false)}.com";
            }            

            return RandomString(stringLength, true);
        }

        // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        public string RandomString(long length, bool includeNonAlpha)
        {
            int repeat = (length > 1000) ? 1000 : (int)length;

            string chars = includeNonAlpha 
                ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ;/\\,.<>!\"££$%^&*()_+=_@{}[]"
                : "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, repeat)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        private DateTime? RandomDate(DateTime start, DateTime end)
        {
            int days = (end - start).Days;
            return start.AddDays(_random.Next(days));
        }

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
