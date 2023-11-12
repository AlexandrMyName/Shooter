using Extentions;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class AsteroidStaticWeapon : MonoBehaviour
{

    [SerializeField] private List<GameObject> _asteroids;

    [SerializeField] private Transform _target;
    [SerializeField] private float _force;
    [SerializeField] private float _lifeTime = 20f;
    [SerializeField] private float _timeSpawn = 5f;


    private List<IDisposable> _disposables = new();


    private void Awake()
    {

        Observable.Timer(TimeSpan.FromSeconds(_timeSpawn)).Repeat().Subscribe(_ =>
        {

            var asteroidInstance = GameObject.Instantiate(_asteroids[UnityEngine.Random.Range(0, _asteroids.Count - 1)],
               new Vector3(UnityEngine.Random.Range(transform.position.x - 200, transform.position.x + 200),
               UnityEngine.Random.Range(transform.position.y - 200, transform.position.y + 200),
               transform.position.z),
                
               transform.rotation);

            float scaleRandom = UnityEngine.Random.Range(1, 50);

            

            var rigidbodies = asteroidInstance.GetComponentsInChildren<Rigidbody>();
            
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
            }

            asteroidInstance.transform.localScale = Vector3.one * scaleRandom;

            asteroidInstance.transform.LookAt(_target);
            var rbMain = asteroidInstance.GetOrAddComponent<Rigidbody>();
            rbMain.isKinematic = false;
            rbMain.useGravity = false;
            rbMain.AddForce(asteroidInstance.transform.forward * _force, ForceMode.Impulse);
            Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(_ =>
            {
                foreach (var rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }
            }).AddTo(_disposables);

            Destroy(asteroidInstance, _lifeTime);
        }).AddTo(_disposables);
    }

}
