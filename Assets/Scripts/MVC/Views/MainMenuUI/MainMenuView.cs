using MVC.Core.Interface.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour, IView
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_Text _progressPointsText;

    public Button StartButton => _startButton;

    public Button LeaderboardButton => _leaderboardButton;

    public Button ExitButton => _exitButton;
    
    public TMP_Text ProgressPointsText => _progressPointsText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
