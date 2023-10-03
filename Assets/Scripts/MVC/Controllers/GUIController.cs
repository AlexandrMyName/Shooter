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
        
        public GUIController(IViewProvider viewProvider)
        {
            _guiView = viewProvider.GetView<GUIView>();
            _pauseView = viewProvider.GetView<PauseView>();
            _gameOverView = viewProvider.GetView<GameOverView>();
        }


        public void Initialisation()
        {
            PlayerEvents.OnDead += ShowGameover;
            PlayerEvents.OnGameEnded += SetScore;
            PlayerEvents.OnGodMode += ToggleGodMode;
            PlayerEvents.OnKeyStatusChanged += ToggleKeyVisual;
            EnemyEvents.OnBossStateChanged += ToggleBossPanel;
            _gameOverView.RestartButton.onClick.AddListener(Restart);
            _gameOverView.ExitButton.onClick.AddListener(Exit);
            _gameOverView.Hide();
            _guiView.KeyIndicatorView.Hide();
            _guiView.BossPanelView.HideHPBar();
        }

        public void Cleanup()
        {
            PlayerEvents.OnDead -= ShowGameover;
            PlayerEvents.OnGameEnded -= SetScore;
            PlayerEvents.OnGodMode -= ToggleGodMode;
            PlayerEvents.OnKeyStatusChanged -= ToggleKeyVisual;
            EnemyEvents.OnBossStateChanged -= ToggleBossPanel;
        }

        private void SetScore(int score)
        {
            _gameOverView.ScoreText.text = score.ToString();
        }

        private void ShowGameover()
        {
            PlayerEvents.PauseGame(true);
            _gameOverView.Show();
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