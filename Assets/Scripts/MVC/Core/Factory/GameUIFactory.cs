using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;

namespace MVC.Core.Factory
{
    public class GameUIFactory : BaseGameFactory
    {
        public GameUIFactory(IControllers controllers, IDataProvider dataProvider, IViewProvider viewProvider,
            IDataFactory dataFactory) : base(controllers, dataProvider, viewProvider, dataFactory)
        {
        }
    }
}