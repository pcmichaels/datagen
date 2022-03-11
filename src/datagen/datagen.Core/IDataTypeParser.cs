using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core
{
    public interface IDataTypeParser
    {
        public bool IsTypeInteger(string type);
        public bool IsTypeDecimal(string type);
        public bool IsTypeString(string type);
        public bool IsTypeDate(string type);
    }
}
