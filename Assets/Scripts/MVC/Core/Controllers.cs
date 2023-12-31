using System.Collections.Generic;
using MVC.Core.Interface.Controllers;
using UnityEngine;

namespace MVC.Core
{
    public sealed class Controllers : IControllers
    {
        private List<IPreInitialisation> _preInitializeControllers;
        private List<IInitialization> _initializeControllers;
        private List<IExecute> _executeControllers;
        private List<IFixedExecute> _fixedExecuteControllers;
        private List<ICleanUp> _cleanupControllers;
        
        internal Controllers()
        {
            Init();
        }
        
        public Controllers Add(IController controller)
        {
            if (controller is IPreInitialisation preInitializeController)
                _preInitializeControllers.Add(preInitializeController);
            
            if (controller is IInitialization initializeController)
                _initializeControllers.Add(initializeController);

            if (controller is IExecute executeController)
                _executeControllers.Add(executeController);

            if (controller is IFixedExecute fixedExecuteController)
                _fixedExecuteControllers.Add(fixedExecuteController);

            if (controller is ICleanUp cleanUpController)
                _cleanupControllers.Add(cleanUpController);
            return this;
        }
        
        private void Init()
        {
            _preInitializeControllers = new List<IPreInitialisation>();
            _initializeControllers = new List<IInitialization>();
            _executeControllers = new List<IExecute>();
            _fixedExecuteControllers = new List<IFixedExecute>();
            _cleanupControllers = new List<ICleanUp>();
        }

        public void PreInitialization()
        {
            for (var i = 0; i < _preInitializeControllers.Count; ++i)
            {
                _preInitializeControllers[i].PreInitialisation();
            }
        }

        public void Initialization()
        {
            for (var i = 0; i < _initializeControllers.Count; ++i)
            {
                _initializeControllers[i].Initialisation();
            }
        }

        public void Execute(float deltaTime)
        {
            for (var index = 0; index < _executeControllers.Count; ++index)
            {
                _executeControllers[index].Execute(deltaTime);
            }
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            for (var index = 0; index < _fixedExecuteControllers.Count; ++index)
            {
                _fixedExecuteControllers[index].FixedExecute(fixedDeltaTime);
            }
        }

        public void Cleanup()
        {
            for (var index = 0; index < _cleanupControllers.Count; ++index)
            {
                _cleanupControllers[index].Cleanup();
            }
        }
    }

    public interface IInitialisation
    {
    }
}
