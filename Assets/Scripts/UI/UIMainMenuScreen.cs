using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class UIMainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private LeaderBoard _leaderBoard;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartButtonClick);
            _leaderboardButton.onClick.AddListener(LeaderboardButtonClick);
            _exitButton.onClick.AddListener(ExitButtonClick);
        }
        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartButtonClick);
            _leaderboardButton.onClick.RemoveListener(LeaderboardButtonClick);
            _exitButton.onClick.RemoveListener(ExitButtonClick);
        }
        private void StartButtonClick()
        {
            SceneManager.LoadScene("ShootArenaWhiteBox");
        }

        private void LeaderboardButtonClick()
        {
            _leaderBoard.Show();
            Hide();
        }
        private void ExitButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
