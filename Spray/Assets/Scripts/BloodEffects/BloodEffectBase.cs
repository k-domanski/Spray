using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class BloodEffectBase : ScriptableObject
{
    public Action<float> OnCurrentDuration;
    public float duration => _duration;
    public float currentDuration
    {
        get =>_currentDuration; 
        set
        {
            _currentDuration = value;
            OnCurrentDuration?.Invoke(value);
        }
    }

    public Sprite icon => _icon;
    public EffectType type => _type;
    [SerializeField] private float _duration;
    [SerializeField] private Sprite _icon;
    [SerializeField] private EffectType _type;

    private float _currentDuration;

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

public enum EffectType
{
    None,
    Buff,
    Debuff
}