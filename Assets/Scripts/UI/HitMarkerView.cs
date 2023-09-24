using EventBus;
using UnityEngine;

namespace UI
{
    public class HitMarkerView : MonoBehaviour
    {
        [SerializeField] private GameObject _hitMarkerObject;
        [SerializeField] private int _markerActiveFrames;

        private bool _isShowed;
        private int _currentShowFrames;
        void Awake()
        {
            Hide();
            EnemyEvents.OnDamaged += ShowTemporary;
        }

        void OnDestroy()
        {
            EnemyEvents.OnDamaged -= ShowTemporary;
        }

        private void FixedUpdate()
        {
            if (_isShowed)
            {
                _currentShowFrames++;
                if (_currentShowFrames > _markerActiveFrames)
                {
                    Hide();
                    _currentShowFrames = 0;
                    _isShowed = false;
                }
            }
        }
    
        private void ShowTemporary()
        {
            Show();
            _isShowed = true;
        }
    
        public void Show()
        {
            _hitMarkerObject.SetActive(true);
        }

        public void Hide()
        {
            _hitMarkerObject.SetActive(false);
        }
    }
}
