using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    public float damage { get; set; }
    public bool canDealDamage { get; set; } = false;
    void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage || other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        var livingEntity = other.GetComponent<LivingEntity>();
        livingEntity.DealDamage(damage, (other.transform.position - transform.position).normalized);
    }
}
