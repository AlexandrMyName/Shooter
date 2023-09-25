using MVC.Core.Interface.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour, IView
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_Text _scoreText;

    public Button RestartButton => _restartButton;

    public Button ExitButton => _exitButton;

    public TMP_Text ScoreText => _scoreText;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}