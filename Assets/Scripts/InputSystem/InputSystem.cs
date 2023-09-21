using Configs;
using EventBus;
using UnityEditor;
using UnityEngine;

namespace InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private GameObject _pauseMenu;
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

                /*#if UNITY_EDITOR
                                if(EditorApplication.isPlaying)
                                {
                                    UnityEditor.EditorApplication.isPaused = true;
                                }
                #else
                            Application.Quit();
                #endif */
            }
        }
        private void ShowPauseMenu()
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }

    }
}