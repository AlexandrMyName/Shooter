using MVC.Core.Interface.View;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Views
{
    public class LeaderBoardView : MonoBehaviour, IView
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _globalButton;
        [SerializeField] private Button _selfButton;
        [SerializeField] private RectTransform _contentRectTransform;

        public Button BackButton => _backButton;

        public Button GlobalButton => _globalButton;

        public Button SelfButton => _selfButton;

        public RectTransform ContentRectTransform => _contentRectTransform;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}