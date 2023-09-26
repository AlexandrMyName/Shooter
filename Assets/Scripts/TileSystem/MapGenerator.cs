using System.Collections.Generic;
using EventBus;
using Extentions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tilePrefabsList;
    [SerializeField] private List<TileView> _tileViews;
    [SerializeField] private TileView _currentTileView;
    
    private void Awake()
    {
        _currentTileView.MapGenerator = this;
    }

    private void Start()
    {
        _tileViews.Clear();
        _tileViews.Add(_currentTileView);
    }

    public void GenerateTile(WorldSide side, ConnectorView connectorView)
    {
        int randomIndex = Extention.GetRandomInt(0, _tilePrefabsList.Count);
        GameObject tile = Instantiate(_tilePrefabsList[randomIndex], gameObject.transform);
        TileView tileView = tile.GetComponent<TileView>();
        _tileViews.Add(tileView);
        tileView.MapGenerator = this;
        
        ConnectorView secondConnectorView;
        if (tileView.TryGetConnector(connectorView.ConnectableSide, out secondConnectorView))
        {
            Vector3 connectorPosition =
                Vector3.zero + connectorView.transform.position;
            tile.transform.position = connectorPosition - secondConnectorView.gameObject.transform.position;
            secondConnectorView.IsGenerated = true;
        }
    }
}
