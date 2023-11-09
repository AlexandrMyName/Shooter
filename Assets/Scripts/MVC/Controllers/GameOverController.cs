using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;

namespace MVC.Controllers
{
    public class GameOverController : IInitialization, ICleanUp
    {
        private GUIView _guiView;
        private PauseView _pauseView;
        private GameOverView _gameOverView;

        public GameOverController(IViewProvider viewProvider)
        {
            _guiView = viewProvider.GetView<GUIView>();
            _pauseView = viewProvider.GetView<PauseView>();
            _gameOverView = viewProvider.GetView<GameOverView>();
        }
        
        public void Initialisation()
        {
            
        }

        public void Cleanup()
        {
            
        }
    }
}