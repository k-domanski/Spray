using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleCollision : MonoBehaviour
{
    #region Private
    private ParticleSystem _particleSystem;
    #endregion

    #region Event
    public UnityEvent<Vector3, Vector3> onParticleCollision;
    #endregion

    #region Messages
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    void OnParticleCollision(GameObject other)
    {
        var collisions = new List<ParticleCollisionEvent>();
        var count = _particleSystem.GetCollisionEvents(other, collisions);

        for (int i = 0; i < count; ++i)
        {
            var pos = collisions[i].intersection;
            var normal = collisions[i].normal;
            onParticleCollision?.Invoke(pos, normal);
            // print($"Collision: [{pos.x}, {pos.y}, {pos.z}]");
        }
    }
    #endregion
}
