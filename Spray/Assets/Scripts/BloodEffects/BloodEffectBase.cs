using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BloodEffectBase : ScriptableObject
{
    public float duration;
    public float currentDuration { get; set; }

    public void RefreshEffect()
    {
        currentDuration = duration;
    }
    public bool IsActive()
    {
        return currentDuration > 0 && currentDuration <= duration;
    }
    public abstract void Apply(GameObject gameObject);
}
