using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DamageDealer : MonoBehaviour
{
    #region Properties
    [SerializeField] private float _damage = 1.0f;
    #endregion

    #region Messages
    void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }
    void OnTriggerEnter(Collider other)
    {
        var livingEntity = other.GetComponent<LivingEntity>();
        if (livingEntity == null)
            return;

        var direction = (livingEntity.transform.position - transform.position).normalized;
        direction.y = 0.0f;

        livingEntity.DealDamage(_damage, direction);
    }
    #endregion
}
