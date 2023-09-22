using Configs;
using EventBus;
using UnityEditor;
using UnityEngine;

namespace InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private InputConfig _inputConfig;
        
        private void Update()
        {
            if (Input.GetKey(_inputConfig.ShootKeyCode))
            {
                ShootingEvents.TryShoot(true);
            }
            else
            {
                ShootingEvents.TryShoot(false);
            }

            if (Input.GetKey(_inputConfig.AimKeyCode))
            {
                ShootingEvents.Aim(true);
            }
            else
            {
                ShootingEvents.Aim(false);
            }

            if (Input.GetKeyDown(_inputConfig.ReloadKeyCode))
            {
                ShootingEvents.Reload();
            }

            if (Input.GetKeyDown(_inputConfig.PauseKeyCode))
            {
                ShowPauseMenu();
            }
        }
        private void ShowPauseMenu()
        {
            PlayerEvents.PauseGame(true);
#if UNITY_EDITOR
            if(EditorApplication.isPlaying) 
            { 
                //UnityEditor.EditorApplication.isPaused = true;
            }
#else
            //Application.Quit();
#endif 
        }

    }
}