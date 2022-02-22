using System;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectController : MonoBehaviour
{
    public Action<BloodEffectBase, Color> onEffectAdded;
    public Action<BloodEffectBase> onEffectRemoved;


    public ColorSampler colorSample => _colorSampler;
    [SerializeField] private BuffLib buffLib;
    private List<BloodEffectBase> _appliedEffects = new List<BloodEffectBase>();
    private Dictionary<ColorType, List<BloodEffectBase>> _availableBuffs = new Dictionary<ColorType, List<BloodEffectBase>>();
    private ColorSampler _colorSampler;
    private void Awake()
    {
        foreach (var pair in buffLib.buffs)
        {
            _availableBuffs.Add(pair.first, pair.second);
        }

        _colorSampler = GetComponent<ColorSampler>();
        _colorSampler.onColorTypeChanged += OnColorChanged;

    }
    public void Update()
    {
        foreach (var effect in _appliedEffects)
        {
            if (effect.IsActive())
            {
                effect.Apply(this.gameObject);
            }
            else
            {
                RemoveEffect(effect);
                break;
            }
        }
    }

    private void OnDisable()
    {
        _colorSampler.onColorTypeChanged -= OnColorChanged;
        foreach(var effect in _appliedEffects)
        {
            effect.Remove();
        }
    }

    public void OnColorChanged(ColorType oldColor, ColorType newColor)
    {
        if (!_availableBuffs.ContainsKey(newColor))
            return;
        var bloodEffects = _availableBuffs[newColor];
        foreach (var bloodEffect in bloodEffects)
        {
            AddEffect(bloodEffect);
        }
    }
    public void AddEffect(BloodEffectBase bloodEffect)
    {
        if (_appliedEffects.Contains(bloodEffect))
        {
            bloodEffect.RefreshEffect();
            return;
        }
        bloodEffect.currentDuration = bloodEffect.duration;
        _appliedEffects.Add(bloodEffect);
        onEffectAdded?.Invoke(bloodEffect, colorSample.bloodColor);
    }

    private void RemoveEffect(BloodEffectBase bloodEffect)
    {
        bloodEffect.Remove();
        _appliedEffects.Remove(bloodEffect);
        onEffectRemoved?.Invoke(bloodEffect);
    }
}
