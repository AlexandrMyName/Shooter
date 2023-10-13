using Core.ResourceLoader;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using MVC.Views;
using SavableData;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MVC.Controllers
{
    public class MainMenuController : IInitialization, ICleanUp
    {
        private MainMenuView _mainMenuView;
        private LeaderBoardView _leaderBoardView;
        
        public MainMenuController(IViewProvider viewProvider)
        {
            _mainMenuView = viewProvider.GetView<MainMenuView>();
            _leaderBoardView = viewProvider.GetView<LeaderBoardView>();
        }
        
        public void Initialisation()
        {
            _mainMenuView.StartButton.onClick.AddListener(StartButtonClick);
            _mainMenuView.LeaderboardButton.onClick.AddListener(LeaderboardButtonClick);
            _mainMenuView.ExitButton.onClick.AddListener(ExitButtonClick);
            _mainMenuView.ProgressPointsText.text =
                ResourceLoadManager.GetConfig<MetaProgression>().ProgressionPoints.ToString();
        }

        public void Cleanup()
        {
            _mainMenuView.StartButton.onClick.RemoveListener(StartButtonClick);
            _mainMenuView.LeaderboardButton.onClick.RemoveListener(LeaderboardButtonClick);
            _mainMenuView.ExitButton.onClick.RemoveListener(ExitButtonClick);
        }
        
        private void StartButtonClick()
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("WhiteBoxTest");
        }

        private void LeaderboardButtonClick()
        {
            _mainMenuView.Hide();
            _leaderBoardView.Show();
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