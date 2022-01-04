using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float damage = 10.0f;
    public float speed = 50.0f;
    public float lifeDuration = 3.0f;
    public GameObject impactEffect;
    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }
        lifeDuration -= Time.deltaTime;
        if (lifeDuration <= 0.0f)
            DestroyBullet();

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        float distanceThisFrame = speed * Time.deltaTime;

        //if (direction.magnitude <= distanceThisFrame * 15)
        //{
        //    HitTarget();
        //    return;
        //}

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        var livingEntity = target.GetComponent<LivingEntity>();
        Debug.Log("livingEntityOwner: " + target.name);
        if (livingEntity != null)
        {
            livingEntity.DealDamage(damage, Vector3.zero);
        }
        DestroyBullet();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;
        if (other.gameObject.TryGetComponent<LivingEntity>(out var livingEntity))
        {
            Debug.Log("livingEntityOwner: " + target.name);
            livingEntity.DealDamage(damage, Vector3.zero);
        }
        DestroyBullet();
    }

    void DestroyBullet()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
        Destroy(gameObject);
    }
}
