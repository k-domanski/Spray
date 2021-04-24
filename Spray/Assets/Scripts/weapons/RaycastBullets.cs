using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBullets : ProjectileBehaviourBase
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particle;
    /*Debug projectile radius gizmo*/
    private Vector3 origintest;
    /*-----------------------------*/


    private void Awake()
    {
        if (_trail != null)
        {
            _trail.startWidth = transform.localScale.x;
        }
        _layer = LayerMask.NameToLayer("Level");
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_destroyNextFrame)
        {
            DestroyProjectile();
            return;
        }

        Vector3 origin = transform.position + _direction * (transform.localScale.x / 2.0f);
        float distance = _speed * Time.fixedDeltaTime;

        /*Debug projectile radius gizmo*/
        origintest = origin;
        /*-----------------------------*/


        RaycastHit hitInfo;
        if (Physics.SphereCast(origin, _raycastRadius, _direction, out hitInfo, distance))
        {
            if (hitInfo.transform.gameObject.layer != _ownerLayer)
            {
                OnProjectileHit(hitInfo);
                distance = hitInfo.distance;
                _destroyNextFrame = true;
            }
        }

        transform.Translate(_direction * distance, Space.World);
    }
    /*Debug projectile radius gizmo*/
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(origintest, _raycastRadius);
    }
    /*-----------------------------*/

    private void DestroyProjectile()
    {
        if (_particle != null && _trail != null)
        {
            _particle.Stop();
            _particle.Clear();
            var emission = _particle.emission;
            emission.enabled = false;
            Destroy(gameObject, _trail.time);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnProjectileHit(RaycastHit hitInfo)
    {
        if (hitInfo.transform.TryGetComponent<LivingEntity>(out var livingEntity))
        {
            var dir = (livingEntity.transform.position - transform.position).normalized;
            dir.y = 0.0f;
            livingEntity.DealDamage(_damage, dir);
            if (hitInfo.transform.TryGetComponent<Enemy>(out var enemy))
                enemy.Knockback(dir, _knockback);
        }
        else
        {
            // print(hitInfo.collider.gameObject.layer);
            if (placeBulletHole && hitInfo.transform.gameObject.layer == _layer)
            {
                if (decalChance == 1.0f)
                {
                    Systems.decalSystem.PlaceBulletHole(hitInfo.point, hitInfo.normal);
                }
                else
                {

                    var roll = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (roll >= decalChance)
                    {
                        Systems.decalSystem.PlaceBulletHole(hitInfo.point, hitInfo.normal);
                    }
                }
            }
        }
    }
}
