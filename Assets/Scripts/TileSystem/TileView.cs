using System;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private int _tileID;
    [SerializeField] private List<ConnectorView> _connectorViews;

    private MapGenerator _mapGenerator;

    public int TileID => _tileID;
    public MapGenerator MapGenerator
    {
        get => _mapGenerator;
        set => _mapGenerator = value;
    }

    private void Awake()
    {
        foreach (ConnectorView connectorView in _connectorViews)
        {
            connectorView.TileView = this;
        }
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
}
