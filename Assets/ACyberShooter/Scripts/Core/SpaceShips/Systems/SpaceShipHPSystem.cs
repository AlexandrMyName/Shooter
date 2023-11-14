using UnityEngine;
using Abstracts;
using UnityEngine.InputSystem;
using UniRx.Triggers;
using UniRx;
using Cinemachine;
using UnityEngine.Rendering;
using MVC;

namespace Core
{

    public class SpaceShipHPSystem : BaseSystem
    {
 
        private ISpaceShip _ship;
        private Volume _deathVolume;
        private bool _isDeath;

        private float _currentTimeToGameOver;
        private bool _gameOver = false;


        protected override void Awake(IGameComponents components)
        {

            _ship = components.BaseObject.GetComponent<SpaceShipComponent>();

            _deathVolume = _ship.DeathPostProccessVolume;


        }


        protected override void Update()
        {

            if (_gameOver) return;

            if(_ship.Health <= 0)
            {
                _ship.Health = 0;
                _isDeath = true;
            }// To ReactProperty ...

            if (_isDeath)
            {
                _deathVolume.weight += Time.deltaTime * _ship.VisualDeathVolumeSpeed;
                _currentTimeToGameOver += Time.deltaTime;
            }

            if(_currentTimeToGameOver >= 2.5f)
            {
                Time.timeScale = 0.3f;
                _ship.Rigidbody.isKinematic = true;
               _ship.Rigidbody.transform.GetComponent<SpaseShip>().enabled = false; // naming space... 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                var deathView = GameObject.Instantiate(_ship.GameUIBehavior.SpaceShipDeathView, _ship.GameUIBehavior.GameUIFactory.CanvasTransform);
                deathView.GetComponent<SpaceShipDeathView>().InitView(_ship.CurrentSceneIndex, _deathVolume);
                _gameOver = true;
            }
        }
    }
}