using MVC.Core.Interface.View;
using UI;
using UnityEngine;

public class GUIView : MonoBehaviour, IView
{
    [SerializeField] private CrosshairView _crosshairView;
    [SerializeField] private HealthPanelView _healthPanelView;
    [SerializeField] private ArmorPanelView _armorPanelView;
    [SerializeField] private AmmoPanelView _ammoPanelView;
    [SerializeField] private GUIScoreView _scoreView;
    [SerializeField] private KeyIndicatorView _keyIndicatorView;
    [SerializeField] private BossPanelView _bossPanelView;

    public CrosshairView CrosshairView => _crosshairView;

    public HealthPanelView HealthPanelView => _healthPanelView;

    public ArmorPanelView ArmorPanelView => _armorPanelView;

    public AmmoPanelView AmmoPanelView => _ammoPanelView;

    public GUIScoreView ScoreView => _scoreView;

    public KeyIndicatorView KeyIndicatorView => _keyIndicatorView;

    public BossPanelView BossPanelView => _bossPanelView;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
