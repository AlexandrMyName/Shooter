using EventBus;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GUIScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentScoreText;
        private int _currentScore;
    
        public void SetCurrentScore(int score)
        {
            _currentScore = score;
            _currentScoreText.text = score.ToString();
        }
    
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _currentScore = 0;
            EnemyEvents.OnDead += AddScore;
        }

        private void OnDestroy()
        {
            EnemyEvents.OnDead -= AddScore;
        }

        private void AddScore()
        {
            _currentScore++;
            _currentScoreText.text = _currentScore.ToString();
        }
    }
}
