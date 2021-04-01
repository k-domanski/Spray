using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBullets : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particle;
    public int damage { get => _damage; set { _damage = value; } }

    private int _damage;
    private Vector3 _direction;
    private float _speed;
    private float _duration;
    private bool _destroyNextFrame = false;
    private LayerMask _layer;

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
            var livingEntity = hitInfo.transform.GetComponent<LivingEntity>();
            if (livingEntity != null)
            {
                var dir = (livingEntity.transform.position - transform.position).normalized;
                dir.y = 0.0f;
                livingEntity.DealDamage(_damage, dir);
            }
            else
            {
                // print(hitInfo.collider.gameObject.layer);
                if (hitInfo.transform.gameObject.layer == _layer)
                    Systems.decalSystem.PlaceBulletHole(hitInfo.point, hitInfo.normal);
            }

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

    public void Fire(Vector3 direction, float speed, float duration)
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        _direction = direction;
        _speed = speed;
        _duration = duration;
        gameObject.SetActive(true);
        Destroy(gameObject, _duration);
    }
}
