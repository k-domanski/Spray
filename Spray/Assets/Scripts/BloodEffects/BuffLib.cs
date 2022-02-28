using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BloodEffects/Buff Library", fileName = "BuffLibrary")]
public class BuffLib : ScriptableObject
{
    public List<Pair<ColorType, List<BloodEffectBase>>> buffs;

    public List<BloodEffectBase> GetBuff(ColorType colorType)
    {
        List<BloodEffectBase> buffList = new List<BloodEffectBase>();
        foreach(var buff in buffs)
        {
            if (buff.first == colorType)
                buffList = buff.second;
        }

        return buffList;
    }
}