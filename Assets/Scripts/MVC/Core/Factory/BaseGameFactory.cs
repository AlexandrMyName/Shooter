using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;

namespace MVC.Core.Factory
{
    public class BaseGameFactory : IFactory
    {
        private protected IControllers _controllers;
        private protected IDataProvider _dataProvider;
        private protected IViewProvider _viewProvider;
        private protected IDataFactory _dataFactory;
        
        
        
        public BaseGameFactory(IControllers controllers, IDataProvider dataProvider, IViewProvider viewProvider,
            IDataFactory dataFactory)
        {
            _controllers = controllers;
            _dataProvider = dataProvider;
            _viewProvider = viewProvider;
            _dataFactory = dataFactory;
            Init();
        }

        private void Init()
        {
            
        }
    }
}