using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core.DataAccessor
{
    public interface IDataAccessorFactory
    {
        T GetDataReader<T>(Func<T> create)
            where T : class;
        T GetDataUpdater<T>(Func<T> create)
            where T : class;
    }
}
