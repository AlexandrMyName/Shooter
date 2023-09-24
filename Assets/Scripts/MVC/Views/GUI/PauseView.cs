using MVC.Core.Interface.View;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour, IView
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    public Button ContinueButton => _continueButton;

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
