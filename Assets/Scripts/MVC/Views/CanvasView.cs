using MVC.Core.Interface.View;
using UnityEngine;

public class CanvasView : MonoBehaviour, IView
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;

    public Canvas Canvas => _canvas;

    public RectTransform RectTransform => _rectTransform;
}
