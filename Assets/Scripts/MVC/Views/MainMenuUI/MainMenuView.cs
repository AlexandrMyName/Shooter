using MVC.Core.Interface.View;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour, IView
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _exitButton;

    public Button StartButton => _startButton;

    public Button LeaderboardButton => _leaderboardButton;

    public Button ExitButton => _exitButton;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
