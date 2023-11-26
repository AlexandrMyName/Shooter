using Core;
using EventBus;
using UnityEditor;
using UnityEngine;


namespace InputSystem
{
    
    public class InputSystem : MonoBehaviour
    {
        
        // [SerializeField] private InputConfig _inputConfig;
        
        private bool isGodModeEnabled = false;

        private PlayerInput _input;
        
        
        
        private void Start()
        {
            _input = NewInput.Instance.PlayerInput;
        }


        private void Update()
        {

            if (_input.Player.Shoot.IsPressed())
            {
                ShootingEvents.TryShoot(true);
            }
            else
            {
                ShootingEvents.TryShoot(false);
            }

            if (_input.Player.Aim.IsPressed())
            {
                ShootingEvents.Aim(true);
            }
            else
            {
                ShootingEvents.Aim(false);
            }

            if (_input.Player.WeaponReload.WasPerformedThisFrame())
            {
              
                ShootingEvents.Reload();
            }
           

            if (_input.System.Pause.IsPressed())
            {
                ShowPauseMenu();
            }
            
            if (_input.Player.GodMode.WasPressedThisFrame())
            {
                isGodModeEnabled = !isGodModeEnabled;
                PlayerEvents.GodMode(isGodModeEnabled);
            }
        }
        
        
        private void ShowPauseMenu()
        {
            PlayerEvents.PauseGame(true);
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                //UnityEditor.EditorApplication.isPaused = true;
            }
#else
            //Application.Quit();
#endif 
        }

        private void OnDestroy()
        {
          //  _input.Dispose();
        }
    }
}