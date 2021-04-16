using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BazookaBullets : ProjectileBehaviourBase
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionRange = 5f;
    private GameObject _explosion;
    
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

        if (Physics.SphereCast(origin,_raycastRadius, _direction, out hitInfo, distance))
        {
            OnProjectileHit(hitInfo);
            distance = hitInfo.distance;
            _destroyNextFrame = true;
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
        Destroy(_explosion, 0.2f);
    }

    public override void OnProjectileHit(RaycastHit hitInfo)
    {
        var entitiesWithinRange = GetEntitiesInRange(hitInfo.point, _explosionRange);
        foreach(LivingEntity entity in entitiesWithinRange)
        {
            var dir = (entity.transform.position - hitInfo.point).normalized;
            dir.y = 0;
            entity.DealDamage(_damage, dir);
        }
        _explosion = Instantiate(_explosionPrefab, hitInfo.point, Quaternion.identity);
        _explosion.gameObject.SetActive(true);
    }

    private List<LivingEntity> GetEntitiesInRange(Vector3 center, float range)
    {
        var entities = FindObjectsOfType<LivingEntity>().
            Where(t => Vector3.Distance(t.transform.position, center) < range).ToList();
        return entities;
    }
}
