using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(Destruct());
    }

    private IEnumerator Destruct()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}