using System;
using Abstracts;
using Configs;
using Extentions;
using RootMotion.Dynamics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


namespace Core
{

    public class PlayerSpaceShipControllSystem : BaseSystem
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;

        private ISpaceShip _ship;

        private bool _isSpaceShipOnly;
        private Collider _defaultSpaceShipCollider;
        
        private PlayerInput _input;
          

        public PlayerSpaceShipControllSystem(bool spaceShipOnly = false, Collider spaceShipCollider = null)
        {

            _isSpaceShipOnly = spaceShipOnly;
            _defaultSpaceShipCollider = spaceShipCollider;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = components.BaseObject.GetComponent<IComponentsStorage>();
            _input = _componentsStorage.Input.PlayerInput;
    
        }


        protected override void Start()
        {

            if (_defaultSpaceShipCollider == null && !_isSpaceShipOnly)
            {
                 
                _components.BaseObject.GetComponent<Collider>()
                    .OnTriggerStayAsObservable()
                    .Subscribe(col =>
                    {
                        if (col.tag == "SpaceShip")
                        {

                            if (_input.Player.Transport.WasPressedThisFrame())
                            {

                                SwitchControll(col);
                                InitSpaceDoors();
                            }
                        }
                    });
            }
            else SwitchControll(_defaultSpaceShipCollider);
  
        }


        private void SwitchControll(Collider col)
        {

             
            _ship = col.GetComponent<ISpaceShip>();
           
            _ship.Camera.gameObject.SetActive(true);
            _ship.Camera.enabled = true;
            _ship.Rigidbody.transform.position += _ship.Rigidbody.transform.forward * 65f;

            _ship.Rigidbody.isKinematic = false;
            _ship.IsLockControll = false;
            _ship.PlayerInput = _input;
        }


        private void InitSpaceDoors()
        {

            _ship.Rigidbody.GetComponent<BoxCollider>().OnTriggerEnterAsObservable().Subscribe(shipCollider =>
            {

                if (shipCollider.tag == "SpaceDoor")
                {

                    if (_ship == null) return;
                    if (_ship.IsLockControll) return;
                    _ship.IsLockControll = true;
                    _ship.PlayerInput = null;
                  
                    var points = shipCollider.GetComponent<SpaceShipPoints>();

                    if (points == null) Debug.Log("Null collider");
                    if (points.ShipIsOut)
                    {
                        _ship.Rigidbody.isKinematic = true;
                        _ship.Rigidbody.transform.position = points._initialPosition;
                        _ship.Rigidbody.transform.rotation = points._initialRotation;
                        _ship.VisualModel.transform.localPosition = Vector3.zero;
                        _ship.VisualModel.transform.localRotation = Quaternion.identity;
                        _ship.Camera.enabled = false;
                        _ship = null;
                        
                    }

                }
            });
        }

    }
}