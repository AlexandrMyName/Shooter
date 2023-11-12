using EventBus;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Controllers
{
    public class GUIController : IInitialization, ICleanUp
    {
        private GUIView _guiView;
        private PauseView _pauseView;
        private GameOverView _gameOverView;
        private WinScreenView _winScreenView;
        
        public GUIController(IViewProvider viewProvider)
        {
            _guiView = viewProvider.GetView<GUIView>();
            _pauseView = viewProvider.GetView<PauseView>();
            _gameOverView = viewProvider.GetView<GameOverView>();
            _winScreenView = viewProvider.GetView<WinScreenView>();
        }


        public void Initialisation()
        {
            PlayerEvents.OnDead += ShowGameover;
            PlayerEvents.OnGameEnded += SetScore;
            PlayerEvents.OnGameWined += ShowWinScreen;
            PlayerEvents.OnGodMode += ToggleGodMode;
            PlayerEvents.OnKeyStatusChanged += ToggleKeyVisual;
            EnemyEvents.OnBossStateChanged += ToggleBossPanel;
            _gameOverView.RestartButton.onClick.AddListener(Restart);
            _gameOverView.ExitButton.onClick.AddListener(Exit);
            _winScreenView.RestartButton.onClick.AddListener(Restart);
            _winScreenView.ExitButton.onClick.AddListener(Exit);
            _gameOverView.Hide();
            _winScreenView.Hide();
            _guiView.KeyIndicatorView.Hide();
            _guiView.BossPanelView.HideHPBar();
        }

        public void Cleanup()
        {
            PlayerEvents.OnDead -= ShowGameover;
            PlayerEvents.OnGameEnded -= SetScore;
            PlayerEvents.OnGameWined -= ShowWinScreen;
            PlayerEvents.OnGodMode -= ToggleGodMode;
            PlayerEvents.OnKeyStatusChanged -= ToggleKeyVisual;
            EnemyEvents.OnBossStateChanged -= ToggleBossPanel;
            _gameOverView.RestartButton.onClick.RemoveListener(Restart);
            _gameOverView.ExitButton.onClick.RemoveListener(Exit);
            _winScreenView.RestartButton.onClick.RemoveListener(Restart);
            _winScreenView.ExitButton.onClick.RemoveListener(Exit);
        }

        private void SetScore(int score, int progressPoints)
        {
            _gameOverView.ScoreText.text = score.ToString();
            _gameOverView.ProgressPoints.text = progressPoints.ToString();
            _winScreenView.ScoreText.text = score.ToString();
            _winScreenView.ProgressPoints.text = progressPoints.ToString();
        }

        private void ShowGameover()
        {
            PlayerEvents.PauseGame(true);
            _gameOverView.Show();
            _pauseView.Hide();
            _guiView.Hide();
        }

        private void ShowWinScreen()
        {
            PlayerEvents.PauseGame(true);
            _winScreenView.Show();
            _pauseView.Hide();
            _guiView.Hide();
        }

        private void ToggleBossPanel(bool isBossAlive)
        {
            if (isBossAlive)
            {
                _guiView.BossPanelView.ShowHPBar();
            }
            else
            {
                _guiView.BossPanelView.HideHPBar();
            }
        }
        
        private void ToggleGodMode(bool isActive)
        {
            if (isActive)
            {
                _guiView.ArmorPanelView.Hide();
                _guiView.HealthPanelView.Hide();
            }
            else
            {
                _guiView.ArmorPanelView.Show();
                _guiView.HealthPanelView.Show();
            }
        }

        private void ToggleKeyVisual(bool hasKey)
        {
            if (hasKey)
            {
                _guiView.KeyIndicatorView.Show();
            }
            else
            {
                _guiView.KeyIndicatorView.Hide();
            }
        }
        
        private void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        private void Exit()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}