using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    #region Properties
    [SerializeField] private Color _lowColor;
    [SerializeField] private Color _middleColor;
    [SerializeField] private Color _highColor;
    [SerializeField] private Image _fillArea;

    public float value
    {
        get => _slider.value;
        set => SetValue(value);
    }
    #endregion

    #region Events
    public event Action<float, float> onValueChanged;
    #endregion

    #region Private
    private Slider _slider;

    #endregion

    #region Messages
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    #endregion

    #region Public
    public void SetValue(float newValue)
    {
        var oldValue = _slider.value;
        _slider.value = newValue;

        if (_fillArea != null)
            _fillArea.color = newValue > 0.5f ? Color.Lerp(_middleColor, _highColor, (newValue * 2f) - 1f) : Color.Lerp(_lowColor, _middleColor, newValue * 2f);

        onValueChanged?.Invoke(oldValue, value);
    }
    #endregion
}
