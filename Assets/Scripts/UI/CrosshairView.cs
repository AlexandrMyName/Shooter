using UnityEngine;

namespace UI
{
    public class CrosshairView : MonoBehaviour
    {
        [SerializeField] private float _scaleChangeDelay;
        [SerializeField] private float _maxScaleDelta;
        [SerializeField] private float _scaleDeltaByStep;
    
        private float _lastChangeTime;
        private bool _isIncreasing;
        private float _currentScaleDelta;

        private void OnEnable()
        {
            _isIncreasing = true;
            _currentScaleDelta = 0;
        }

        public void TryChangeScale(bool isShooting)
        {
            if (isShooting)
            {
                if (_lastChangeTime + _scaleChangeDelay < Time.time)
                {
                    _lastChangeTime = Time.time;
                    if (_isIncreasing)
                    {
                        _isIncreasing = false;
                        if (_currentScaleDelta < _maxScaleDelta)
                        {
                            _currentScaleDelta += _scaleDeltaByStep;
                        }
                        ChangeCrosshairScale(_currentScaleDelta);
                    }
                    else
                    {
                        _isIncreasing = true;
                        if (_currentScaleDelta > 0)
                        {
                            _currentScaleDelta -= _scaleDeltaByStep/2;
                        }
                        ChangeCrosshairScale(_currentScaleDelta);
                    }
                }
            }
            else
            {
                if (_currentScaleDelta > 0)
                {
                    _currentScaleDelta -= _scaleDeltaByStep/2;
                }
                ChangeCrosshairScale(_currentScaleDelta);
            }

        }

        private void ChangeCrosshairScale(float scaleDelta)
        {
            Vector3 corsshairRecoiledScale = gameObject.transform.localScale;
            corsshairRecoiledScale = new Vector3(1 + scaleDelta,
                1 + scaleDelta, corsshairRecoiledScale.z);
            gameObject.transform.localScale = corsshairRecoiledScale;
        }
    }
}
