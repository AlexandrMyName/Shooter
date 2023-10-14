using Configs;
using Core.ResourceLoader;
using MVC.Core.Factory;
using MVC.Core.Initialization;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Factory;
using MVC.Core.Interface.Providers;
using MVC.Core.Providers;
using UnityEngine;

namespace MVC.Core
{
    public class GameMonoBeh : MonoBehaviour
    {
        [SerializeField] private ConfigLoader _configLoader;
        
        private IControllers _controllers;
        private IDataProvider _dataProvider;
        private IViewProvider _viewProvider;
        private IDataFactory _dataFactory;

        private void Awake()
        {
            ResourceLoadManager.Init(_configLoader);
            _controllers = new Controllers();
            _dataProvider = new DataProvider();
            _viewProvider = new ViewProvider();
            _dataFactory = new DataFactory(_dataProvider);
            
            Saver.Init(_configLoader); 
            GameUIFactory gameUIFactory = new GameUIFactory(_controllers, _dataProvider, _viewProvider, _dataFactory);
            GameUIInitialization uiInitialization = new GameUIInitialization(gameUIFactory);
            _controllers.PreInitialization();
        }
        void Start()
        {
            _controllers.Initialization();
        }
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            _controllers.Execute(deltaTime);
        }
        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            _controllers.FixedExecute(fixedDeltaTime);
        }
        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}
