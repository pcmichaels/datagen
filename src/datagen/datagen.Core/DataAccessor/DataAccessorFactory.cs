using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core.DataAccessor
{
    public class DataAccessorFactory : IDataAccessorFactory
    {
        private object? _dataReader = null;
        private object? _dataUpdater = null;       

        public T GetDataReader<T>(Func<T> create) 
            where T : class
        {
            if (_dataReader == null)
                _dataReader = create();

            return (T)_dataReader;

        }

        public T GetDataUpdater<T>(Func<T> create)
            where T : class
        {
            if (_dataUpdater == null)
                _dataUpdater = create();

            return (T)_dataUpdater;

        }
    }
}
