using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;

namespace MVC.Core.Factory
{
    public class BaseGameFactory : IFactory
    {
        private IControllers _controllers;
        private IDataProvider _dataProvider;
        private IViewProvider _viewProvider;
        private IDataFactory _dataFactory;
        
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