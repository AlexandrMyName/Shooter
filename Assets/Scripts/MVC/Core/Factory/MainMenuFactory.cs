using Core.ResourceLoader;
using MVC.Controllers;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;
using UnityEngine;

namespace MVC.Core.Factory
{
    public class MainMenuFactory : BaseGameFactory
    {
        private RectTransform _canvasTransform;
        public MainMenuFactory(IControllers controllers, IDataProvider dataProvider, IViewProvider viewProvider,
            IDataFactory dataFactory) : base(controllers, dataProvider, viewProvider, dataFactory)
        {
        }

        public void CreateCanvas()
        {
            GameObject canvas = 
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("Canvas"));
            CanvasView canvasView = canvas.GetComponent<CanvasView>();
            canvasView.Canvas.worldCamera = Camera.main;
            _viewProvider.AddView(canvasView);
            _canvasTransform = canvasView.RectTransform;
        }

        public void CreateMainMenuPanel()
        {
            GameObject mainMenuPanel =
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("MainMenuScreen"),
                    _canvasTransform);
            MainMenuView mainMenuView = mainMenuPanel.GetComponent<MainMenuView>();
            _viewProvider.AddView(mainMenuView);
            _controllers.Add(new MainMenuController(_viewProvider));
        }
        
    }
}