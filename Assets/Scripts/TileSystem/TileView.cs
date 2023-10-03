using System.Collections.Generic;
using EnemySystem;
using EventBus;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private int _tileID;
    [SerializeField] private List<ConnectorView> _connectorViews;
    [SerializeField] private Transform _foundationTransform;
    [SerializeField] private BossSpawner _bossSpawner;
    
    private Vector2 _cornerNW;
    private Vector2 _cornerNE;
    private Vector2 _cornerSW;
    private Vector2 _cornerSE;
    private MapGenerator _mapGenerator;

    public int TileID => _tileID;
    public MapGenerator MapGenerator
    {
        get => _mapGenerator;
        set => _mapGenerator = value;
    }
    public Transform FoundationTransform => _foundationTransform;

    public BossSpawner BossSpawner => _bossSpawner;

    public Vector2 CornerNW => _cornerNW;

    public Vector2 CornerNE => _cornerNE;

    public Vector2 CornerSW => _cornerSW;

    public Vector2 CornerSE => _cornerSE;
    

    private void Awake()
    {
        Initialize();
    }

    public bool TryGetConnector(WorldSide side, out ConnectorView sideConnectorView)
    {
        bool isConnectorExists = false;
        sideConnectorView = null;
        foreach (ConnectorView connectorView in _connectorViews)
        {
            if (connectorView.WorldSide == side)
            {
                sideConnectorView = connectorView;
                isConnectorExists = true;
            }
        }
        return isConnectorExists;
    }

    public void Initialize()
    {
        float halfFoundationScaleX = _foundationTransform.localScale.x / 2;
        float halfFoundationScaleY = _foundationTransform.localScale.z / 2;
        _cornerNW = new Vector2(-halfFoundationScaleX, halfFoundationScaleY);
        _cornerNE = new Vector2(halfFoundationScaleX, halfFoundationScaleY);
        _cornerSW = new Vector2(-halfFoundationScaleX, -halfFoundationScaleY);
        _cornerSE = new Vector2(halfFoundationScaleX, -halfFoundationScaleY);
        
        foreach (ConnectorView connectorView in _connectorViews)
        {
            connectorView.TileView = this;
            connectorView.CheckConnectorRequirements();
        }
    }
}
