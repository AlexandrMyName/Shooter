using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/EnemySystem/SpawnConfig", order = 2)]
public class SpawnConfig : ScriptableObject
{
    [SerializeField] private List<Vector3> _spawnPointsList;
    [SerializeField] private bool _isInfiniteSpawning;
    [SerializeField] private int _totalSpawnCount;
    [SerializeField] private int _waveSpawnCount;
    [SerializeField] private int _waveCooldown;

    public List<Vector3> SpawnPointsList => _spawnPointsList;

    public bool IsInfiniteSpawning => _isInfiniteSpawning;

    public int TotalSpawnCount => _totalSpawnCount;

    public int WaveSpawnCount => _waveSpawnCount;

    public int WaveCooldown => _waveCooldown;

    [Button]
    private void AddSelectedAsSpawnPoints()
    {
        foreach (Transform pointTransform in Selection.transforms)
        {
            _spawnPointsList.Add(pointTransform.position);
        }
    }

    [Button]
    private void ClearSpawnPoints()
    {
        _spawnPointsList.Clear();
    }
}
