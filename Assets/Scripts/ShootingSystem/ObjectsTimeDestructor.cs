using System.Collections;
using UnityEngine;

public class ObjectsTimeDestructor : MonoBehaviour
{
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
}