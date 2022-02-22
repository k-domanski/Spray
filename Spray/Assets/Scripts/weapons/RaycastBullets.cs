using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBullets : ProjectileBehaviourBase
{
    [SerializeField] private Texture2D _holeTexture = null;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Brush _brush;
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
            int objectLayerMask = (1 << hitInfo.transform.gameObject.layer);
            if ((objectLayerMask & collisionLayers.value) != 0)
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
            Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
            if (paintable)
            {
                //PaintData data = new PaintData();
                //data.color = Color.black;
                //data.brush = _holeTexture;
                //data.radius = UnityEngine.Random.Range(0.2f, 0.35f);
                //data.rotation = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);

                PaintData data = _brush.GetPaintData();
                data.color = Color.black;
                Systems.paintManager.Paint(paintable, hitInfo.point, data);
            }
        }
    }
}
