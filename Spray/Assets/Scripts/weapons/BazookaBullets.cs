using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BazookaBullets : IProjectileBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private GameObject _explosionPrefab;

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

        RaycastHit hitInfo;

        if (Physics.Raycast(origin, _direction, out hitInfo, distance))
        {
            OnProjectileHit(hitInfo);

            distance = hitInfo.distance;
            _destroyNextFrame = true;
        }

        transform.Translate(_direction * distance, Space.World);
    }

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
        var entitiesWithinRange = FindObjectsOfType<LivingEntity>().Where(t => Vector3.Distance(t.transform.position, hitInfo.point) < 5f).ToList();
        foreach(LivingEntity entity in entitiesWithinRange)
        {
            var dir = (entity.transform.position - hitInfo.point).normalized;
            dir.y = 0;
            entity.DealDamage(_damage, dir);
        }
        var explo = Instantiate(_explosionPrefab, hitInfo.transform.position, Quaternion.identity);
        explo.gameObject.SetActive(true);
    }
}
