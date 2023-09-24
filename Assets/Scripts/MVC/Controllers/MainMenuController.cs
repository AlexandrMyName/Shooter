using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using UnityEngine.SceneManagement;

namespace MVC.Controllers
{
    public class MainMenuController : IInitialization, ICleanUp
    {
        private MainMenuView _mainMenuView;
        
        public MainMenuController(IViewProvider viewProvider)
        {
            _mainMenuView = viewProvider.GetView<MainMenuView>();
        }
        
        public void Initialisation()
        {
            _mainMenuView.StartButton.onClick.AddListener(StartButtonClick);
            _mainMenuView.LeaderboardButton.onClick.AddListener(LeaderboardButtonClick);
            _mainMenuView.ExitButton.onClick.AddListener(ExitButtonClick);
        }

        public void Cleanup()
        {
            _mainMenuView.StartButton.onClick.RemoveListener(StartButtonClick);
            _mainMenuView.LeaderboardButton.onClick.RemoveListener(LeaderboardButtonClick);
            _mainMenuView.ExitButton.onClick.RemoveListener(ExitButtonClick);
        }
        
        private void StartButtonClick()
        {
            SceneManager.LoadScene("ShootArenaWhiteBox");
        }

        private void LeaderboardButtonClick()
        {

        }
        private void ExitButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}