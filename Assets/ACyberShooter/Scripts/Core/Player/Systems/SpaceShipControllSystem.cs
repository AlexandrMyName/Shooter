using System;
using Abstracts;
using Configs;
using RootMotion.Dynamics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{

    public class SpaceShipControllSystem : BaseSystem
    {

        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private IAnimatorIK _animatorIK;
        private ISpaceShip _ship;


        private Rigidbody _rb;

        private Vector3 _direction = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
        private float _currentAcceleration;
        private bool _isLockControll = true;
        private bool _isSpaceShipOnly;
        private Collider _defaultSpaceShipCollider;
        private Vector3 _initialPosition;
        
        private PlayerInput _input;
          

        public SpaceShipControllSystem(bool spaceShipOnly = false, Collider spaceShipCollider = null)
        {

            _isSpaceShipOnly = spaceShipOnly;
            _defaultSpaceShipCollider = spaceShipCollider;
        }


        protected override void Awake(IGameComponents components)
        {

            _components = components;
            _componentsStorage = components.BaseObject.GetComponent<IComponentsStorage>();
            _input = _componentsStorage.Input.PlayerInput;
           
            _rotation = Vector3.zero;
            _animatorIK = _componentsStorage.AnimatorIK;
 
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
            else
            {
                SwitchControll(_defaultSpaceShipCollider);
 
            }
        }


        private void SwitchControll(Collider col)
        {

            // _components.BaseObject.GetComponent<Rigidbody>().useGravity = false;
            _ship = col.GetComponent<ISpaceShip>();
            //  _components.BaseObject.transform.parent.transform.parent.transform.parent = _ship.GetTransform();
            // _components.BaseObject.transform.position = _ship.GetTransform().position;
            _ship.Camera.gameObject.SetActive(true);
            _ship.Camera.enabled = true;
            _ship.Rigidbody.transform.position += _ship.Rigidbody.transform.forward * 25f;

            _ship.Rigidbody.isKinematic = false;
            _isLockControll = false;
            
        }


        private void InitSpaceDoors()
        {

            _ship.Rigidbody.GetComponent<Collider>().OnTriggerEnterAsObservable().Subscribe(shipCollider =>
            {

                if (shipCollider.tag == "SpaceDoor")
                {
                    if (_isLockControll) return;
                    _isLockControll = true;
                    // shipCollider.enabled = false;
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


        protected override void FixedUpdate()
        {
            
            if (_ship == null || _isLockControll) return;
            
            Vector3 delta = _input.Player.Mouse.ReadValue<Vector2>();
            float x_axis
                = delta.y *
                (Gamepad.all.Count > 0
                ? _componentsStorage.CinemachineCameraConfig.Sensetivity_GamePad
                : _componentsStorage.CinemachineCameraConfig.Sensetivity_Mouse) * 0.002f * Time.fixedDeltaTime;

            float y_axis = delta.x * (Gamepad.all.Count > 0
                ? _componentsStorage.CinemachineCameraConfig.Sensetivity_GamePad
                : _componentsStorage.CinemachineCameraConfig.Sensetivity_Mouse) * Time.fixedDeltaTime;

            _ship.Camera.m_YAxis.Value -= x_axis;
            _ship.Camera.m_XAxis.Value += y_axis;
            
            _ship.Camera.LookAt.transform.eulerAngles
              = new Vector3(
                  _ship.Camera.m_YAxis.Value,
                   _ship.Camera.m_XAxis.Value,
                   0);



            MoveSpaceship();
            TurnSpaceship();
            //CalculateShootingLogic();
        }

        //public ObjectPoolBehaviour projectileObjectPool;
        //public Transform projectileSpawnTransform;
        //private float nextShot = 0.0f;
        //public Transform shipModel;

        

        void MoveSpaceship()
        {

            _currentAcceleration =  _input.SpaceShip.Acceleration.ReadValue<float>() * _ship.SpaceshipData.thrustAmountSprint;
            
            _ship.Rigidbody.velocity 
                = _ship.Rigidbody.transform.forward
                * _ship.SpaceshipData.thrustAmount * (_currentAcceleration == 0 ? 1 : _currentAcceleration)
                * (Mathf.Max(_ship.SpaceshipData.thrustInput, .2f));
        }


        void TurnSpaceship()
        {
            Vector3 newTorque 
                = new Vector3(_ship.SpaceshipData.steeringInput.x * _ship.SpaceshipData.pitchSpeed, -_ship.SpaceshipData.steeringInput.z * _ship.SpaceshipData.yawSpeed, 0);
            _ship.Rigidbody.AddRelativeTorque(newTorque);

            _ship.Rigidbody.rotation =
                Quaternion.Slerp(_ship.Rigidbody.rotation,
                Quaternion.Euler(new Vector3(_ship.Rigidbody.transform.localEulerAngles.x, _ship.Rigidbody.transform.localEulerAngles.y, 0)), .5f);

            VisualSpaceshipTurn();
        }


        void VisualSpaceshipTurn()
        {
            _ship.VisualModel.localEulerAngles = new Vector3(_ship.SpaceshipData.steeringInput.x * _ship.SpaceshipData.leanAmount_Y
                , _ship.VisualModel.localEulerAngles.y, _ship.SpaceshipData.steeringInput.z * _ship.SpaceshipData.leanAmount_X);
        }

        //void CalculateShootingLogic()
        //{
        //    if (data.shootInput == true)
        //    {
        //        if (Time.time > nextShot)
        //        {
        //            ShootProjectile();
        //            nextShot = Time.time + data.shootRate;
        //        }
        //    }
        //}

        //void ShootProjectile()
        //{

        //    GameObject newProjectile = projectileObjectPool.GetPooledObject();
        //    newProjectile.transform.position = projectileSpawnTransform.position;
        //    newProjectile.transform.rotation = projectileSpawnTransform.rotation;
        //    newProjectile.SetActive(true);

        //}

         
        private Vector3 rawInputSteering;
        private Vector3 smoothInputSteering;

        //Thrust
         
        private float rawInputThrust;
        private float smoothInputThrust;

        //Shooting
        private bool shootHeld;

      
        
        public void OnShoot(InputAction.CallbackContext value)
        {
            if (value.started)
            {
                shootHeld = true;
            }
            else if (value.canceled)
            {
                shootHeld = false;
            }
        }

        protected override void Update()
        {
            if (_ship == null || _isLockControll) return;

          
            Vector2 rawInput = _input.Player.Move.ReadValue<Vector2>();
            rawInputSteering = new Vector3(rawInput.y, 0, -rawInput.x);
            //rawInputThrust = value.ReadValue<float>();
            InputSmoothing();
            SetInputData();
        }
        

        void InputSmoothing()
        {
            //Steering
            smoothInputSteering = Vector3.Lerp(smoothInputSteering, rawInputSteering, Time.deltaTime * _ship.SpaceshipData.SteeringSmoothing);

            //Thrust
            smoothInputThrust = Mathf.Lerp(smoothInputThrust, rawInputThrust, Time.deltaTime * _ship.SpaceshipData.ThrustSmoothing);
        }

        void SetInputData()
        {
            _ship.SpaceshipData.UpdateInputData(smoothInputSteering, smoothInputThrust, shootHeld);
        }
    }
}