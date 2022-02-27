using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffColorLevel : MonoBehaviour
{
    [SerializeField] private BuffLib _buffLib;
    private List<BloodEffectBase> _appliedEffects = new List<BloodEffectBase>();
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void Update()
    {
        foreach (var buff in _appliedEffects)
        {
            if (buff.IsActive())
            {
                buff.Apply(this.gameObject);
                _player.colorLevels[buff.colorType] = (buff.currentDuration / buff.duration) * _player.maxColorLevel;
            }
            else
            {
                RemoveBuff(buff);
                break;
            }
        }
    }
    private void OnDisable()
    {
        foreach(var buff in _appliedEffects)
        {
            buff.Remove();
        }
    }
    private void RemoveBuff(BloodEffectBase buff)
    {
        buff.Remove();
        _appliedEffects.Remove(buff);
    }

    public void UseBuff(float value)
    {

        switch (value)
        {
            case 1:
                if(_player.colorLevels[ColorType.Red] == _player.maxColorLevel)
                {
                    AddBuff(_buffLib.GetBuff(ColorType.Red), ColorType.Red);
                }
                break;
            case 2:
                if (_player.colorLevels[ColorType.Green] == _player.maxColorLevel)
                {
                    AddBuff(_buffLib.GetBuff(ColorType.Green), ColorType.Green);
                }
                break;
            case 3:
                if (_player.colorLevels[ColorType.Yellow] == _player.maxColorLevel)
                {
                    AddBuff(_buffLib.GetBuff(ColorType.Yellow), ColorType.Yellow);
                }
                break;
            default:
                break;
        }
    }

    private void AddBuff(List<BloodEffectBase> buffs, ColorType color)
    {
        foreach (var buff in buffs)
        {
            _appliedEffects.Add(buff);
            buff.colorType = color;
            buff.currentDuration = buff.duration;
        }
    }
}
