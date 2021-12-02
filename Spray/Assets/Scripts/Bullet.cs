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
        float distanceThisFrame = speed * Time.deltaTime;

        if(direction.magnitude <= distanceThisFrame * 15)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        DestroyBullet();
        var livingEntity = target.parent.GetComponent<LivingEntity>();
        Debug.Log("livingEntityOwner: " + target.parent.name);
        if(livingEntity != null)
        {
            livingEntity.DealDamage(damage, Vector3.zero);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
    }
}
