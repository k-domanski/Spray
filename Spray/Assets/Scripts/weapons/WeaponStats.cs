using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapons/WeaponStats")]
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

    [Header("Overheating")]
    [SerializeField] private float _cooldownFactor;
    [SerializeField] private float _cooldownActivationTime;
    [SerializeField] private float _maxHeatValue;
    [SerializeField] private float _heatStepPerShot;

    [SerializeField] private string _ownerLayer;


    public float fireRate => _fireRate;
    public int damage => _damage; 
    public float knockback => _knockback;
    public ProjectileBehaviourBase projectile  => _projectile;

    public bool hasRecoil => _hasRecoil;

    public float accuracy => _accuracy;
    public float speed => _speed;
    public float playerBaseSpeedReduction => _playerBaseSpeedReduction;
    public float playerSpeedReductionWhileShooting => _playerSpeedReductionWhileShooting;

    public float cooldownFactor => _cooldownFactor;
    public float cooldownActivationTime => _cooldownActivationTime;
    public float maxHeatValue => _maxHeatValue;
    public float heatStepPerShot => _heatStepPerShot;

    public void CreateProjectile(Vector3 position, Vector3 direction, float playerMultiplier, bool placeBulletHole = true, float decalChance = 1.0f)
    {
        var projectileObject = GameObject.Instantiate(_projectile.gameObject, position, Quaternion.identity);

        var targetRotation = Quaternion.LookRotation(direction);
        projectileObject.transform.localRotation = Quaternion.Lerp(projectileObject.transform.localRotation,
                                                                   targetRotation,
                                                                   1.0f);
        var projectile = projectileObject.GetComponent<ProjectileBehaviourBase>();
        projectile.placeBulletHole = placeBulletHole;
        projectile.decalChance = decalChance;
        projectile.raycastRadius = _projectileRaycastRadius;
        projectile.damage = _damage * playerMultiplier;
        projectile.ownerLayer = LayerMask.NameToLayer(_ownerLayer);
        projectile.Fire(direction, _speed, _duration, _knockback);
    }
}
