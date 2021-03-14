using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFX : MonoBehaviour
{
    #region Properties
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int _particleCount;
    #endregion
    #region Public
    public void CastEffect(LivingEntity entity)
    {
        _particleSystem.Emit(1);
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
    #endregion
}
