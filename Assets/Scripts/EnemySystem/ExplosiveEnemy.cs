using System.Collections;
using System.Collections.Generic;
using EnemySystem;
using RootMotion.Dynamics;
using Player;
using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyView;
    public float explosionRadius = 20f;
    public float explosionDelay = 2f;
    private bool isPlayerTouching;
    private bool hasDamagedPlayer = false;
    [SerializeField] private bool damageAllies = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll") && _enemyView.IsDead == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerRagdoll"))
                {
                    isPlayerTouching = true;
                    StartCoroutine(ExplodeAfterDelay());
                }
            }
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        if (!_enemyView.IsDead)
        {      
        if (isPlayerTouching && !hasDamagedPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _enemyView.PlayerView.PlayerDamagableZoneTransform.position);
            if (distanceToPlayer <= explosionRadius)
            {
                _enemyView.PlayerView.RagdollStun();
                _enemyView.PlayerView.Invoke("RagdollUnStun", 0.5f);
                _enemyView.PlayerView.TakeDamage(10);
                hasDamagedPlayer = true;
            }
        }
        if (damageAllies)
        {
            EnemyView[] enemies = FindObjectsOfType<EnemyView>();
            foreach (EnemyView enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= explosionRadius)
                {
                    enemy.TakeDamage(9999);
                }
            }
        }
        Destroy(gameObject);
        _enemyView.TakeDamage(9999);
        }
    }
}