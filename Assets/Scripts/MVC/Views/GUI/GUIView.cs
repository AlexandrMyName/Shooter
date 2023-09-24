using MVC.Core.Interface.View;
using UI;
using UnityEngine;

public class GUIView : MonoBehaviour, IView
{
    [SerializeField] private CrosshairView _crosshairView;
    [SerializeField] private HealthPanelView _healthPanelView;
    [SerializeField] private AmmoPanelView _ammoPanelView;
    [SerializeField] private GUIScoreView _scoreView;

    public CrosshairView CrosshairView => _crosshairView;

    public HealthPanelView HealthPanelView => _healthPanelView;

    public AmmoPanelView AmmoPanelView => _ammoPanelView;

    public GUIScoreView ScoreView => _scoreView;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
