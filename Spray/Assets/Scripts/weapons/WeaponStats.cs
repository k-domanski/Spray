using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponStats", menuName ="Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;

    [SerializeField] private float _knockback;
    [SerializeField] private RaycastBullets _projectile;

    public float fireRate { get =>_fireRate; } 
    public int damage { get =>_damage; } 
    public float knockback { get =>_knockback; } 
    public RaycastBullets projectile { get =>_projectile; } 

    public void CreateProjectile(Vector3 position, Vector3 direction)
    {
        var projectileObject = GameObject.Instantiate(_projectile.gameObject, position, Quaternion.identity);

        var targetRotation = Quaternion.LookRotation(direction);
        projectileObject.transform.localRotation = Quaternion.Lerp(projectileObject.transform.localRotation,
                                                                   targetRotation,
                                                                   1.0f);
        var projectile = projectileObject.GetComponent<RaycastBullets>();
        projectile.damage = _damage;
        projectile.Fire(direction, _speed, _duration);
    }
}
