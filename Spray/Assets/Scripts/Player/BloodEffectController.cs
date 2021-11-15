using System.Collections.Generic;
using UnityEngine;

public class BloodEffectController : MonoBehaviour
{
    [SerializeField] private BuffLib buffLib;
    private List<BloodEffectBase> _appliedEffects = new List<BloodEffectBase>();
    private Dictionary<ColorType, List<BloodEffectBase>> _availableBuffs = new Dictionary<ColorType, List<BloodEffectBase>>();
    private ColorSampler _colorSampler;
    private void Awake()
    {
        foreach (var pair in buffLib.buffs)
        {
            _availableBuffs.Add(pair.firts, pair.second);
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
    }

    private void RemoveEffect(BloodEffectBase bloodEffect)
    {
        bloodEffect.Remove();
        _appliedEffects.Remove(bloodEffect);
    }
}
