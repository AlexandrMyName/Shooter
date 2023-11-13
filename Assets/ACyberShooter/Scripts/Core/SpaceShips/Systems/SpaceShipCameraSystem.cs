using UnityEngine;
using Abstracts;
using UnityEngine.InputSystem;
using UniRx.Triggers;
using UniRx;
using Cinemachine;

namespace Core
{

    public class SpaceShipCameraSystem : BaseSystem
    {
 
        private ISpaceShip _ship;
        

        protected override void Awake(IGameComponents components)
        {
 
            _ship = components.BaseObject.GetComponent<SpaceShipComponent>();

            var collider = _ship.Rigidbody.transform.GetComponent<Collider>();

            collider.OnTriggerStayAsObservable().Subscribe(col =>
            {
                if(col.gameObject.tag == "ShakeCamera")
                {

                    _ship.CameraWithShake.gameObject.SetActive(true);
                }
            });//AddTo

            collider.OnTriggerExitAsObservable().Subscribe(col =>
            {
                if (col.gameObject.tag == "ShakeCamera")
                {

                    _ship.CameraWithShake.gameObject.SetActive(false);
                }
            });//AddTo
        }
 
        protected override void FixedUpdate()
        {

            if (_ship.IsLockControll || _ship.PlayerInput == null) return;

            Vector3 delta = _ship.PlayerInput.Player.Mouse.ReadValue<Vector2>();
            float x_axis
                = delta.y *
                (Gamepad.all.Count > 0
                ?_ship.CameraConfig.Sensetivity_GamePad
                : _ship.CameraConfig.Sensetivity_Mouse) * 0.002f * Time.fixedDeltaTime;
            Debug.Log(Gamepad.all.Count > 0);
            float y_axis = delta.x * (Gamepad.all.Count > 0
                ? _ship.CameraConfig.Sensetivity_GamePad
                : _ship.CameraConfig.Sensetivity_Mouse) * Time.fixedDeltaTime;

            _ship.Camera.m_YAxis.Value -= x_axis;
            _ship.Camera.m_XAxis.Value += y_axis;

        }

    }
}