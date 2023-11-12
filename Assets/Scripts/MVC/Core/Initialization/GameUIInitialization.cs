using MVC.Core.Factory;

namespace MVC.Core.Initialization
{
    public class GameUIInitialization
    {
        private GameUIFactory _gameUIFactory;
        
        public GameUIInitialization(GameUIFactory gameUIFactory)
        {
            _gameUIFactory = gameUIFactory;
            Initialization();
        }

        private void Initialization()
        {
            _gameUIFactory.CreateCanvas();
            _gameUIFactory.CreateGUIPanel();
            _gameUIFactory.CreatePauseMenuPanel();
            _gameUIFactory.CreateGameOverPanel();
            _gameUIFactory.CreateWinPanel();
            _gameUIFactory.CreateGUIControllers();
        }
    }
}