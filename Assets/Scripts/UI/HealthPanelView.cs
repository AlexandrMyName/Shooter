using TMPro;
using UnityEngine;

public class HealthPanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentHPText;
    [SerializeField] private TMP_Text _maxHPText;


    public void SetCurrentHP(int hp)
    {
        _currentHPText.text = hp.ToString();
    }

    public void SetMaxHP(int hp)
    {
        _maxHPText.text = hp.ToString();
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
