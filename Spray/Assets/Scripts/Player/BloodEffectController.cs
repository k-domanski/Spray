using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodEffectController : MonoBehaviour
{
    private List<BloodEffectBase> _appliedEffects = new List<BloodEffectBase>();

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
        bloodEffect.currentDuration = 0;
        _appliedEffects.Remove(bloodEffect);
    }
}
