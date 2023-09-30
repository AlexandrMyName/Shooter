using EventBus;
using Player;
using UnityEngine;

public class ConnectorView : MonoBehaviour
{
    [SerializeField] private WorldSide _worldSide;
    private TileView _tileView;
    private bool _isGenerated;
    private WorldSide _connectableSide;
    private float _leftSideSpaceRequired;
    private float _rightSideSpaceRequired;
    private float _backSideSpaceRequired;
    private bool _isUsable;
    private PlayerView _playerView;
    
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

    public void CheckConnectorRequirements()
    {
        Vector3 connectorLocalPosition = new Vector3(
            gameObject.transform.position.x - _tileView.FoundationTransform.position.x,
            gameObject.transform.position.y,
            gameObject.transform.position.z - _tileView.FoundationTransform.position.z);
        switch (_worldSide)
        {
            case WorldSide.North:
                _connectableSide = WorldSide.South;
                _leftSideSpaceRequired = connectorLocalPosition.x - _tileView.CornerNW.x;
                _rightSideSpaceRequired = _tileView.CornerNE.x - connectorLocalPosition.x;
                _backSideSpaceRequired = _tileView.FoundationTransform.localScale.z / 2 + connectorLocalPosition.z;
                break;
            case WorldSide.South:
                _connectableSide = WorldSide.North;
                _leftSideSpaceRequired = _tileView.CornerSE.x - connectorLocalPosition.x;
                _rightSideSpaceRequired = connectorLocalPosition.x - _tileView.CornerSW.x;
                _backSideSpaceRequired = _tileView.FoundationTransform.localScale.z / 2 - connectorLocalPosition.z;
                break;
            case WorldSide.West:
                _connectableSide = WorldSide.East;
                _leftSideSpaceRequired = connectorLocalPosition.z - _tileView.CornerSW.y;
                _rightSideSpaceRequired = _tileView.CornerNW.y - connectorLocalPosition.z;
                _backSideSpaceRequired = _tileView.FoundationTransform.localScale.x / 2 - connectorLocalPosition.x;
                break;
            case WorldSide.East:
                _connectableSide = WorldSide.West;
                _leftSideSpaceRequired = _tileView.CornerNE.y - connectorLocalPosition.z;
                _rightSideSpaceRequired = connectorLocalPosition.z - _tileView.CornerSE.y;
                _backSideSpaceRequired = _tileView.FoundationTransform.localScale.x / 2 + connectorLocalPosition.x;
                break;
            default:
                _connectableSide = WorldSide.None;
                Debug.Log($"Invalid connector side {gameObject}");
                break;
        }
    }

    public bool IsConnectionPossible(ConnectorView connectorView)
    {
        bool isPossible;
        TileView tileView = connectorView.TileView;
        Vector3 foundationScale = tileView.FoundationTransform.localScale;
        Vector3 overlapCenter = gameObject.transform.position;
        float x = overlapCenter.x - connectorView.gameObject.transform.position.x +
                  tileView.FoundationTransform.position.x;
        float y = overlapCenter.y - foundationScale.y / 2;
        float z = overlapCenter.z - connectorView.gameObject.transform.position.z +
                  tileView.FoundationTransform.position.z;
        overlapCenter = new Vector3(x, y, z);
        Vector3 overlapHalfExtents = new Vector3(foundationScale.x / 2 - 1, foundationScale.y / 2 - 1,
            foundationScale.z / 2 - 1);
        Collider[] hitColliders = Physics.OverlapBox(overlapCenter, overlapHalfExtents, Quaternion.identity);
        if (hitColliders.Length == 0)
        {
            isPossible = true;
        }
        else
        {
            isPossible = false;
        }
        return isPossible;
    }

    private void Update()
    {
        if (_isUsable && Input.GetKeyDown(KeyCode.E))
        {
            _isGenerated = true;
            PlayerEvents.ChangeKeyStatus(false);
            _tileView.MapGenerator.GenerateTile(_worldSide, this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isGenerated)
        {
            other.gameObject.TryGetComponent(out PlayerBoneView playerBoneView);
            _playerView = playerBoneView.PlayerView;
            if (_playerView.HasKey)
            {
                _isUsable = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && !_isGenerated)
        {
            _isUsable = false;
        }
    }
}
