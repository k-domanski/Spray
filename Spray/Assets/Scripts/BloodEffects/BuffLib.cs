using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BloodEffects/Buff Library", fileName = "BuffLibrary")]
public class BuffLib : ScriptableObject
{
    public List<Pair<ColorType, List<BloodEffectBase>>> buffs;
}