using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [Header("Weapon Related")]
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _knockback;

    [Header("Projectile Related")]
    [SerializeField] private ProjectileBehaviourBase _projectile;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileDuration;
    [SerializeField] private float _projectileRaycastRadius;
    [SerializeField] private string _ownerLayer;
    [SerializeField] private ProjectileCount _projectileCount = ProjectileCount.ONE;
    [Tooltip("Angle in radians PI/value \nHigher values = smaller angle"), Range(1, 500)]
    [SerializeField] private int _spreadAngle = 96;


    [Header("Recoil")]
    [SerializeField] private bool _hasRecoil;
    [Range(1f, 200f)]
    [SerializeField] private float _accuracy;

    [Header("Player Related")]
    [SerializeField] private float _playerBaseSpeedReduction;
    [SerializeField] private float _playerSpeedReductionWhileShooting;

    [Header("Overheating")]
    [SerializeField] private float _cooldownSpeed;
    [SerializeField] private float _cooldownActivationTime;
    [SerializeField] private float _maxHeatValue;
    [SerializeField] private float _heatStepPerShot;


    //Weapon Related
    public float fireRate => _fireRate;
    public int damage => _damage; 
    public float knockback => _knockback;

    //Projectile Related
    public ProjectileBehaviourBase projectile  => _projectile;
    public float projectileSpeed => _projectileSpeed;
    public ProjectileCount projectileCount {get => _projectileCount; set =>_projectileCount = value; }
    public int spreadAngle => _spreadAngle;

    //Recoil Related
    public bool hasRecoil => _hasRecoil;
    public float accuracy => _accuracy;

    //Player Related
    public float playerBaseSpeedReduction => _playerBaseSpeedReduction;
    public float playerSpeedReductionWhileShooting => _playerSpeedReductionWhileShooting;

    //Overheating
    public float cooldownSpeed => _cooldownSpeed;
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
        projectile.Fire(direction, _projectileSpeed, _projectileDuration, _knockback);
    }
}

//Just to have drop down menu TODO: write custom inspector GUI/change so it can have even numbers
public enum ProjectileCount
{
    ONE = 1,
    THREE = 3,
    FIVE = 5,
    SEVEN = 7,
    TWENTYONE = 21
}
