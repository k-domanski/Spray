using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    #region Properties
    [SerializeField] private EnemyVisualsData _visualData;
    [SerializeField] private BloodFX _bloodFX;
    #endregion

    #region Private
    private Material _baseMaterial;
    private Material _particleMaterial;
    private Material _splatMaterial;
    private LivingEntity _livingEntity;
    private Renderer _renderer;
    #endregion

    #region Messages
    void OnEnable()
    {
        _bloodFX.particleCollision.onParticleCollision.AddListener(PlaceSplatWithMaterial);
    }
    void OnDisable()
    {
        _bloodFX.particleCollision.onParticleCollision.RemoveListener(PlaceSplatWithMaterial);
    }
    void Awake()
    {
        _livingEntity = GetComponent<LivingEntity>();
        _renderer = GetComponent<Renderer>();
        _baseMaterial = _renderer.material;
        _baseMaterial.color = _visualData.baseColor;

        _particleMaterial = Instantiate(_visualData.particlesMaterial);
        _splatMaterial = Instantiate(_visualData.splatMaterial);

        _particleMaterial.color = _visualData.baseColor;
        _splatMaterial.color = _visualData.baseColor;

    }
    void Start()
    {
        _bloodFX.SetMaterial(_particleMaterial);
    }
    void OnDestroy()
    {
        Destroy(_particleMaterial);
        Destroy(_splatMaterial);
        Destroy(_baseMaterial);
    }
    #endregion

    #region Private Methods
    private void PlaceSplatWithMaterial(Vector3 position, Vector3 facing)
    {
        Systems.decalSystem.PlaceSplat(position, facing, _splatMaterial);
    }
    #endregion
}
