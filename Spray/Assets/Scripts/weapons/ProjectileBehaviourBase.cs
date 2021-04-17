using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehaviourBase : MonoBehaviour
{
    /* TODO: move to a struct */
    public int damage { get => _damage; set { _damage = value; } }
    public LayerMask ownerLayer { get => _ownerLayer; set { _ownerLayer = value; } }
    public float raycastRadius { get => _raycastRadius; set { _raycastRadius = value; } }
    public bool placeBulletHole { get; set; } = true;


    protected int _damage;
    protected Vector3 _direction;
    protected float _speed;
    protected float _duration;
    protected float _knockback;
    protected float _raycastRadius;
    protected bool _destroyNextFrame = false;
    protected LayerMask _layer;
    protected LayerMask _ownerLayer;

    public void Fire(Vector3 direction, float speed, float duration, float knockback)
    {
        if (gameObject.activeSelf)
        {
            return;
        }
        _direction = direction;
        _speed = speed;
        _duration = duration;
        _knockback = knockback;
        gameObject.SetActive(true);
        Destroy(gameObject, _duration);
    }

    public abstract void OnProjectileHit(RaycastHit hitInfo);
}
