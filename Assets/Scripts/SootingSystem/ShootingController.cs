using System;
using UnityEditor;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Mouse0;
    public GameObject ball;
    public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    public Vector3 force = new Vector3(0f, 0f, 7f);
    public float mass = 0f;
    public int delay;
    private bool _isShooting = false;
    private int _timeUntillShoot;


    private void Start()
    {
        _timeUntillShoot = 0;
    }

    private void Update () 
    {
        if (Input.GetKey(keyCode))
        {
            _isShooting = true;
        }
        else
        {
            _isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            if(EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPaused = true;
            }
#endif
        }
    }

    private void FixedUpdate()
    {
        if (_isShooting && _timeUntillShoot <= 0)
        {
            GameObject b = (GameObject)GameObject.Instantiate(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
            var r = b.GetComponent<Rigidbody>();

            if (r != null) 
            {
                Transform cameraTransform = Camera.main.transform;
                r.mass = mass;
                r.AddForce(Quaternion.LookRotation(cameraTransform.forward) * force, ForceMode.VelocityChange);
            }
            _timeUntillShoot = delay;
        }

        if (_timeUntillShoot > 0)
        {
            _timeUntillShoot--;
        }
    }
}
