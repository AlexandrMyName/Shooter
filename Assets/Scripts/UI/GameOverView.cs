using EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _scoreText;
    
        private void Awake()
        {
            PlayerEvents.OnDead += ShowGameover;
            PlayerEvents.OnGameEnded += SetScore;
            _restartButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(Exit);
            Hide();
        }

        private void OnDestroy()
        {
            PlayerEvents.OnDead -= ShowGameover;
            PlayerEvents.OnGameEnded -= SetScore;
        }

        private void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void ShowGameover()
        {
            PlayerEvents.PauseGame(true);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
