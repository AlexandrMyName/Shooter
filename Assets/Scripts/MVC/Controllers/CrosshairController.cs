using EventBus;
using MVC.Core.Interface.Controllers;
using MVC.Core.Interface.Providers;
using UnityEngine;

namespace MVC.Controllers
{
    public class CrosshairController : IInitialization, ICleanUp, IFixedExecute
    {
        private CrosshairView _crosshairView;
        
        private bool _isHitShowed;
        private int _currentShowHitFrames;
        private float _lastChangeTime;
        private bool _isIncreasing;
        private float _currentScaleDelta;

        public CrosshairController(IViewProvider viewProvider)
        {
            _crosshairView = viewProvider.GetView<GUIView>().CrosshairView;
        }
        
        public void Initialisation()
        {
            _isIncreasing = true;
            _currentScaleDelta = 0;
            _currentShowHitFrames = 0;
            _crosshairView.HitMarkerObject.SetActive(false);
            EnemyEvents.OnDamaged += ShowHitTemporary;
        }
        
        public void FixedExecute(float fixedDeltaTime)
        {
            if (_isHitShowed)
            {
                _currentShowHitFrames++;
                if (_currentShowHitFrames > _crosshairView.MarkerActiveFrames)
                {
                    _crosshairView.HitMarkerObject.SetActive(false);
                    _currentShowHitFrames = 0;
                    _isHitShowed = false;
                }
            }
        }

        public void Cleanup()
        {
            EnemyEvents.OnDamaged -= ShowHitTemporary;
        }
        
        private void ShowHitTemporary()
        {
            _crosshairView.HitMarkerObject.SetActive(true);
            _isHitShowed = true;
        }

        public void TryChangeScale(bool isShooting)
        {
            if (isShooting)
            {
                if (_lastChangeTime + _crosshairView.ScaleChangeDelay < Time.time)
                {
                    _lastChangeTime = Time.time;
                    if (_isIncreasing)
                    {
                        _isIncreasing = false;
                        if (_currentScaleDelta < _crosshairView.MaxScaleDelta)
                        {
                            _currentScaleDelta += _crosshairView.ScaleDeltaByStep;
                        }
                        ChangeCrosshairScale(_currentScaleDelta);
                    }
                    else
                    {
                        _isIncreasing = true;
                        if (_currentScaleDelta > 0)
                        {
                            _currentScaleDelta -= _crosshairView.ScaleDeltaByStep/2;
                        }
                        ChangeCrosshairScale(_currentScaleDelta);
                    }
                }
            }
            else
            {
                if (_currentScaleDelta > 0)
                {
                    _currentScaleDelta -= _crosshairView.ScaleDeltaByStep/2;
                }
                ChangeCrosshairScale(_currentScaleDelta);
            }

        }

        private void ChangeCrosshairScale(float scaleDelta)
        {
            Vector3 corsshairRecoiledScale = _crosshairView.CrosshairObject.transform.localScale;
            corsshairRecoiledScale = new Vector3(1 + scaleDelta,
                1 + scaleDelta, corsshairRecoiledScale.z);
            _crosshairView.CrosshairObject.transform.localScale = corsshairRecoiledScale;
        }


    }
}