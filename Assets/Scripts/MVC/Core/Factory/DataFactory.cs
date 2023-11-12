using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;

namespace MVC.Core.Factory
{
    public class DataFactory : IDataFactory
    {
        private IDataProvider _dataProvider;
        
        public DataFactory(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

    }
}