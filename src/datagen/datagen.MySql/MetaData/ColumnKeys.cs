using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.MySql.MetaData
{
    internal class ColumnKeys
    {
        public string Constraint_Name { get; set; }
        public string Column_Name { get; set; }
        public string Referenced_Table_Schema { get; set; }
        public string Referenced_Table_Name { get; set; }
        public string Referenced_Column_Name { get; set; }
    }
}
