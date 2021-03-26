using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFX : MonoBehaviour
{
    #region Properties
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleCollision _particleCollision;
    [SerializeField] private int _particleCount;
    public ParticleCollision particleCollision => _particleCollision;
    #endregion


    #region Messages
    #endregion

    #region Public
    public void CastEffect(LivingEntity entity)
    {
        _particleSystem.Emit(1);
    }
    public void CastInDirection(float damage, Vector3 direction)
    {
        var emitParams = new ParticleSystem.EmitParams();
        _particleSystem.transform.forward = direction;
        _particleSystem.Emit(emitParams, _particleCount);
    }
    [ContextMenu("Emit")]
    public void Emit()
    {
        var emitParams = new ParticleSystem.EmitParams();
        var x = Random.Range(-1.0f, 1.0f);
        var z = Random.Range(-1.0f, 1.0f);
        _particleSystem.transform.forward = new Vector3(x, 0, z);
        // var rotation = Quaternion.FromToRotation(_particleSystem.transform.up, Vector3.)
        _particleSystem.Emit(emitParams, _particleCount);
    }
    public void SetMaterial(Material material)
    {
        var renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = material;
    }
    #endregion
}
