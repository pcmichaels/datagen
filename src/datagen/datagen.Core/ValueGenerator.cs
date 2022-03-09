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

        public ValueGenerator(bool isRandom)
        {
            _isRandom = isRandom;
        }

        public object GenerateValue(string columnName, string dataType, bool isNullable)
        {
            switch (dataType)
            {
                case "varchar":
                    return "1";                    

                case "datetime":
                    return DateTime.MaxValue.AddDays(-1);

                case "int":
                    return int.MaxValue;
            }

            return 1;
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
