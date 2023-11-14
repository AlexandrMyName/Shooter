using Core.ResourceLoader;
using MVC.Controllers;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;
using UnityEngine;


namespace MVC.Core.Factory
{
    public class GameUIFactory : BaseGameFactory
    {
        
        public RectTransform CanvasTransform;


        public GameUIFactory(IControllers controllers, IDataProvider dataProvider, IViewProvider viewProvider,
            IDataFactory dataFactory) : base(controllers, dataProvider, viewProvider, dataFactory)
        { }
        
        public void CreateCanvas()
        {
            GameObject canvas = 
                GameObject.Instantiate(ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("Canvas"));
            CanvasView canvasView = canvas.GetComponent<CanvasView>();
            canvasView.Canvas.worldCamera = Camera.main;
            canvasView.Canvas.planeDistance = 3.08f;
            _viewProvider.AddView(canvasView);
            CanvasTransform = canvasView.RectTransform;
        }

        public void CreateGUIPanel()
        {
            GameObject guiMain = 
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("GUI_Main"),
                    CanvasTransform);
            GUIView guiView = guiMain.GetComponent<GUIView>();
            _viewProvider.AddView(guiView);
        }

        public void CreatePauseMenuPanel()
        {
            GameObject pauseMenuPanel = 
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("PauseMenu"),
                    CanvasTransform);
            PauseView pauesView = pauseMenuPanel.GetComponent<PauseView>();
            _viewProvider.AddView(pauesView);
        }

        public void CreateGameOverPanel()
        {
            GameObject gameOverPanel = 
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("GameOverPanel"),
                    CanvasTransform);
            GameOverView gameOverView = gameOverPanel.GetComponent<GameOverView>();
            _viewProvider.AddView(gameOverView);
        }
        
        public void CreateWinPanel()
        {
            GameObject winPanel = 
                GameObject.Instantiate(
                    ResourceLoadManager.GetPrefabComponentOrGameObject<GameObject>("WinPanel"),
                    CanvasTransform);
            WinScreenView winScreenView = winPanel.GetComponent<WinScreenView>();
            _viewProvider.AddView(winScreenView);
        }

        public void CreateGUIControllers()
        {
            _controllers.Add(new GUIController(_viewProvider));
            _controllers.Add(new PauseMenuController(_viewProvider));
            _controllers.Add(new GameOverController(_viewProvider));
            _controllers.Add(new CrosshairController(_viewProvider));
        }
    }
}