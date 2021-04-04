using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponStats", menuName ="Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private RaycastBullets _projectile;
    
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;

    //[Range(0, 1)]
    [SerializeField] private float _knockback;

    [Header("Recoil")]
    [SerializeField] private bool _hasRecoil;
    [Range(1f, 200f)]
    [SerializeField] private float _accuracy;
    

    public float fireRate { get =>_fireRate; } 
    public int damage { get =>_damage; } 
    public float knockback { get =>_knockback; } 
    public RaycastBullets projectile { get =>_projectile; } 

    public bool hasRecoil { get => _hasRecoil; }

    public float accuracy { get => _accuracy; }

    public void CreateProjectile(Vector3 position, Vector3 direction)
    {
        var projectileObject = GameObject.Instantiate(_projectile.gameObject, position, Quaternion.identity);

        var targetRotation = Quaternion.LookRotation(direction);
        projectileObject.transform.localRotation = Quaternion.Lerp(projectileObject.transform.localRotation,
                                                                   targetRotation,
                                                                   1.0f);
        var projectile = projectileObject.GetComponent<RaycastBullets>();
        projectile.damage = _damage;
        projectile.Fire(direction, _speed, _duration, _knockback);
    }
}
