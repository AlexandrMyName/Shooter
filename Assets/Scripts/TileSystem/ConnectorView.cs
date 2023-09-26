using System;
using EventBus;
using UnityEngine;

public class ConnectorView : MonoBehaviour
{
    [SerializeField] private WorldSide _worldSide;
    private TileView _tileView;
    private bool _isGenerated;
    private WorldSide _connectableSide;
    public WorldSide WorldSide => _worldSide;

    public WorldSide ConnectableSide => _connectableSide;

    public TileView TileView
    {
        get => _tileView;
        set => _tileView = value;
    }
    public bool IsGenerated
    {
        get => _isGenerated;
        set => _isGenerated = value;
    }

    private void Awake()
    {
        switch (_worldSide)
        {
            case WorldSide.North:
                _connectableSide = WorldSide.South;
                break;
            case WorldSide.South:
                _connectableSide = WorldSide.North;
                break;
            case WorldSide.West:
                _connectableSide = WorldSide.East;
                break;
            case WorldSide.East:
                _connectableSide = WorldSide.West;
                break;
            default:
                _connectableSide = WorldSide.None;
                Debug.Log($"Invalid connector side {gameObject}");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isGenerated)
        {
            _tileView.MapGenerator.GenerateTile(_worldSide, this);
            _isGenerated = true;
            gameObject.SetActive(false);
        }
    }
}
