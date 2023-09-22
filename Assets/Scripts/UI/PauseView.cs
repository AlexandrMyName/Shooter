using EventBus;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        Time.timeScale = 1.0f;
        PlayerEvents.OnGamePaused += ChangePauseState;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerEvents.OnGamePaused -= ChangePauseState;
    }

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
    }
    private void OnDisable()
    {
        _continueButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }

    private void ChangePauseState(bool isPaused)
    {
        gameObject.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        
    }
    private void ContinueButtonClick()
    {
        PlayerEvents.PauseGame(false);
    }
    private void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
