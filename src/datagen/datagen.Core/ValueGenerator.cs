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

        public ValueGenerator(bool isRandom)
        {
            _isRandom = isRandom;
        }

        public object GenerateValue(string columnName, string dataType, bool isNullable)
        {
            if (dataType == "varchar")
                return "1";

            if (dataType == "datetime")
                return "2022-03-02";

            return 1;
        }

        public int IntGeneric(bool allowNulls)
        {
            throw new NotImplementedException();
        }

        public int IntGeneric(string fieldName, bool allowNulls)
        {
            throw new NotImplementedException();
        }
    }
}
