using UnityEngine;

public class CrosshairView : MonoBehaviour
{
    [SerializeField] private GameObject _crosshairObject;
    [SerializeField] private GameObject _hitMarkerObject;
    [SerializeField] private int _markerActiveFrames;
    [SerializeField] private float _scaleChangeDelay;
    [SerializeField] private float _maxScaleDelta;
    [SerializeField] private float _scaleDeltaByStep;

    public GameObject CrosshairObject => _crosshairObject;

    public GameObject HitMarkerObject => _hitMarkerObject;

    public int MarkerActiveFrames => _markerActiveFrames;

    public float ScaleChangeDelay => _scaleChangeDelay;

    public float MaxScaleDelta => _maxScaleDelta;

    public float ScaleDeltaByStep => _scaleDeltaByStep;
}
