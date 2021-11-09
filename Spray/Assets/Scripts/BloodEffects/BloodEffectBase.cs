using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BloodEffectBase : ScriptableObject
{
    public float duration => _duration;
    public float currentDuration { get; set; }

    [SerializeField] private float _duration;

    public void RefreshEffect()
    {
        currentDuration = _duration;
    }
    public bool IsActive()
    {
        return currentDuration > 0 && currentDuration <= _duration;
    }
    public abstract void Apply(GameObject gameObject);
    public abstract void Remove();

    protected void Tick()
    {
        currentDuration -= Time.deltaTime;
    }
}
