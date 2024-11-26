using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public Collider damageCollider;
    public int damageAmount = 20;

    public string targetTag; //tag Enemy
    //danh sach cac collider enemy
    public List<Collider> colliderTargets = new List<Collider>();

    void Start()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie") && !colliderTargets.Contains(other))
        {
            colliderTargets.Add(other);
            var enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie") && !colliderTargets.Contains(other))
        {
            colliderTargets.Add(other);
            var enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }
        }
    }

    public void BeginAttack()
    {
        colliderTargets.Clear();
        damageCollider.enabled = true;
    }

    public void EndAttack()
    {
        colliderTargets.Clear();
        damageCollider.enabled = false;
    }
}
