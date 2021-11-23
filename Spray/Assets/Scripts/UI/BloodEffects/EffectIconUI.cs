using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectIconUI : MonoBehaviour
{
    public Color borderColor
    {
        set => _border.color = value;
    }
    public Image icon
    {
        get => _effectIcon;
        set => _effectIcon = value;
    }

    [SerializeField] private Image _effectIcon;
    [SerializeField] private TextMeshProUGUI _durationText;
    private Image _border;

    void Awake()
    {
        _border = GetComponent<Image>();
    }

    public void SetDuration(float duration)
    {
        var timeSpan = TimeSpan.FromSeconds(duration);
        _durationText.text = $"{timeSpan.Seconds}:{timeSpan.Milliseconds}";
    }
}
