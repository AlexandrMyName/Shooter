using EventBus;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Controllers
{
    public class PauseMenuController : IInitialization, ICleanUp
    {
        private int _currentScore;
        private GUIView _guiView;
        private PauseView _pauseView;
        private GameOverView _gameOverView;

        public PauseMenuController(IViewProvider viewProvider)
        {
            _guiView = viewProvider.GetView<GUIView>();
            _pauseView = viewProvider.GetView<PauseView>();
            _gameOverView = viewProvider.GetView<GameOverView>();
        }

        public void Initialisation()
        {
            Time.timeScale = 1f;
            PlayerEvents.OnGamePaused += ChangePauseState;
            EnemyEvents.OnDead += AddScore;
            _pauseView.ContinueButton.onClick.AddListener(ContinueButtonClick);
            _pauseView.ExitButton.onClick.AddListener(ExitButtonClick);
        }

        public void Cleanup()
        {
            PlayerEvents.OnGamePaused -= ChangePauseState;
            EnemyEvents.OnDead -= AddScore;
            _pauseView.ContinueButton.onClick.RemoveAllListeners();
            _pauseView.ExitButton.onClick.RemoveAllListeners();
        }

        private void ChangePauseState(bool isPaused)
        {
            if (isPaused)
            {
                Time.timeScale = 0f;
                _guiView.Hide();
                _pauseView.Show();
                _gameOverView.Hide();
            }
            else
            {
                Time.timeScale = 1.0f;
                _guiView.Show();
                _pauseView.Hide();
                _gameOverView.Hide();
            }
        }
    
        private void ContinueButtonClick()
        {
            PlayerEvents.PauseGame(false);
        }
        private void ExitButtonClick()
        {
            PlayerEvents.GameEnded(_currentScore);
            SceneManager.LoadScene("MainMenu");
        }
    
        private void AddScore()
        {
            _currentScore++;
        }
    }
}