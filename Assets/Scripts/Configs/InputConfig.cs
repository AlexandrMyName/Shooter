using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputSystem/InputConfig", order = 1)]
    
    public class InputConfig : ScriptableObject
    {
        [SerializeField] private KeyCode _shootKeyCode = KeyCode.Mouse0;
        [SerializeField] private KeyCode _aimKeyCode = KeyCode.Mouse1;
        [SerializeField] private KeyCode _reloadKeyCode = KeyCode.R;
        [SerializeField] private KeyCode _pauseKeyCode = KeyCode.Escape;
        
        public KeyCode ShootKeyCode => _shootKeyCode;
        public KeyCode AimKeyCode => _aimKeyCode;
        public KeyCode ReloadKeyCode => _reloadKeyCode;
        public KeyCode PauseKeyCode => _pauseKeyCode;
        
    }
}