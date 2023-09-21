using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    private void OnEnable()
    {
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
    }
    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);
    }
    private void ContinueButtonClick()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
    private void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
