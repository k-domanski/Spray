using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponStats", menuName ="Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [SerializeField] private ProjectileBehaviourBase _projectile;
    
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;
    [SerializeField] private float _projectileRaycastRadius;

    //[Range(0, 1)]
    [SerializeField] private float _knockback;

    [Header("Recoil")]
    [SerializeField] private bool _hasRecoil;
    [Range(1f, 200f)]
    [SerializeField] private float _accuracy;

    [Header("Player related")]
    [SerializeField] private float _playerBaseSpeedReduction;
    [SerializeField] private float _playerSpeedReductionWhileShooting;

    [SerializeField] private string _ownerLayer;


    public float fireRate { get =>_fireRate; } 
    public int damage { get =>_damage; } 
    public float knockback { get =>_knockback; } 
    public ProjectileBehaviourBase projectile { get =>_projectile; } 

    public bool hasRecoil { get => _hasRecoil; }

    public float accuracy { get => _accuracy; }
    public float playerBaseSpeedReduction => _playerBaseSpeedReduction;
    public float playerSpeedReductionWhileShooting => _playerSpeedReductionWhileShooting;

    public void CreateProjectile(Vector3 position, Vector3 direction)
    {
        var projectileObject = GameObject.Instantiate(_projectile.gameObject, position, Quaternion.identity);

        var targetRotation = Quaternion.LookRotation(direction);
        projectileObject.transform.localRotation = Quaternion.Lerp(projectileObject.transform.localRotation,
                                                                   targetRotation,
                                                                   1.0f);
        var projectile = projectileObject.GetComponent<ProjectileBehaviourBase>();
        projectile.raycastRadius = _projectileRaycastRadius;
        projectile.damage = _damage;
        projectile.ownerLayer = LayerMask.NameToLayer(_ownerLayer);
        projectile.Fire(direction, _speed, _duration, _knockback);
    }
}
