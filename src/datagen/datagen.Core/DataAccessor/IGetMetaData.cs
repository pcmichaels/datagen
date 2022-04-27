using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core.DataAccessor
{
    public interface IGetMetaData
    {
        public IEnumerable<Dictionary<string, object>> GetColumnDefinitions(Dictionary<string, string> parameters);

        public IEnumerable<Dictionary<string, object>> GetColumnData(Dictionary<string, string> parameters);
    }
}
