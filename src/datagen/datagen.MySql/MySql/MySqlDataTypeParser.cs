using datagen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql
{
    public class MySqlDataTypeParser : IDataTypeParser
    {
        public bool IsTypeDate(string type)
        {
            switch (type)
            {
                case "date":
                case "datetime":
                case "timestamp":
                    return true;
                default:
                    return false;
            }
        }

        public bool IsTypeDecimal(string type)
        {
            return (type == "decimal");
        }

        public bool IsTypeInteger(string type)
        {
            switch (type)
            {
                case "int":
                case "tinyint":
                    return true;
                default:
                    return false;
            }
        }

        public bool IsTypeString(string type)
        {
            switch (type)
            {
                case "char":
                case "varchar":
                case "longtext":
                    return true;
                default:
                    return false;
            }

        }
    }
}