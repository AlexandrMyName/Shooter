using UnityEngine;


public class EnemyJoint : MonoBehaviour
{

    [SerializeField] private Rigidbody _pelvisPhysics;


    public void AddExplosionForce(Vector3 direction, float force, float radius)
    => _pelvisPhysics.AddExplosionForce(force, direction, radius,100f,ForceMode.Impulse);
     
}
