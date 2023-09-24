using MVC.Core.Factory;

namespace MVC.Core.Initialization
{
    public class MainMenuInitialization
    {
        private MainMenuFactory _menuUIFactory;
        
        public MainMenuInitialization(MainMenuFactory menuUIFactory)
        {
            _menuUIFactory = menuUIFactory;
            Initialization();
        }

        private void Initialization()
        {
            _menuUIFactory.CreateCanvas();
            _menuUIFactory.CreateMainMenuPanel();
        }     
    }
}