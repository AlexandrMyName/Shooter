using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerView : MonoBehaviour
{
    [SerializeField] private Transform _spawnerTransform;

    public Transform SpawnerTransform => _spawnerTransform;
    public Vector3 SpawnerPosition => _spawnerTransform.position;
}
