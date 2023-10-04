using System.Collections.Generic;
using EnemySystem;
using EventBus;
using Extentions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tilePrefabsList;
    [SerializeField] private List<GameObject> _bossTilePrefabsList;
    [SerializeField] private List<TileView> _tileViews;
    [SerializeField] private TileView _currentTileView;
    [SerializeField] private SpawningSystem _spawningSystem;
    [SerializeField] private int _tilesToSpawnBoss;
    
    private void Awake()
    {
        _currentTileView.MapGenerator = this;
    }

    private void Start()
    {
        _tileViews.Clear();
        _tileViews.Add(_currentTileView);
        _currentTileView.BossSpawner.SpawningSystem = _spawningSystem;
    }

    public void GenerateTile(WorldSide side, ConnectorView connectorView)
    {
        List<GameObject> tilePrefabsList;
        if (_tilesToSpawnBoss <= 0)
        {
            tilePrefabsList = _bossTilePrefabsList;
        }
        else
        {
            tilePrefabsList = _tilePrefabsList;
            _tilesToSpawnBoss--;
        }
        
        int index = Extention.GetRandomInt(0, tilePrefabsList.Count);
        GameObject prefab = tilePrefabsList[index];
        TileView prefabView = prefab.GetComponent<TileView>();
        int t = 0;
        while (prefabView.TileID == connectorView.TileView.TileID || t > 10)
        {
            index = Extention.GetRandomInt(0, tilePrefabsList.Count);
            prefab = tilePrefabsList[index];
            prefabView = prefab.GetComponent<TileView>();
            t++;
        }
        
        prefabView.Initialize();
        ConnectorView secondConnectorView;
        if (prefabView.TryGetConnector(connectorView.ConnectableSide, out secondConnectorView))
        {
            bool isEnoughSpace = connectorView.IsConnectionPossible(secondConnectorView);
            if (isEnoughSpace)
            {
                SpawnTile(index, connectorView, tilePrefabsList);
            }
            secondConnectorView.IsGenerated = true;
            connectorView.gameObject.SetActive(false);
        }
    }

    private void SpawnTile(int index, ConnectorView connectorView, List<GameObject> tilePrefabsList)
    {
        PlayerEvents.ChangeKeyStatus(false);
        GameObject tile = Instantiate(tilePrefabsList[index], gameObject.transform);
        TileView tileView = tile.GetComponent<TileView>();
        _tileViews.Add(tileView);
        tileView.MapGenerator = this;
        tileView.BossSpawner.SpawningSystem = _spawningSystem;
        ConnectorView secondConnectorView;
        if (tileView.TryGetConnector(connectorView.ConnectableSide, out secondConnectorView))
        {
            Vector3 connectorPosition =
                Vector3.zero + connectorView.transform.position;
            tile.transform.position = connectorPosition - secondConnectorView.gameObject.transform.position;
        }
    }
}
