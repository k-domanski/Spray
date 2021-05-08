using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    #region Properties
    [SerializeField] private ScoreUIProxy _proxy;
    [SerializeField] private TextMeshProUGUI _scoreValue;
    [SerializeField] private TextMeshProUGUI _scoreMultiplier;
    [SerializeField] private Slider _multiplierSlider;
    #endregion

    #region Messages
    void Awake()
    {
        _proxy.Register(this);
    }
    #endregion

    #region Public
    public void SetScoreValue(float value)
    {
        value = Mathf.Clamp(value, 0, 999999999);
        var str = ((int)value).ToString();
        _scoreValue.text = str;
    }
    public void SetScoreMultiplier(float value)
    {
        value = Mathf.Clamp(value, 1, 999);
        var str = ((int)value).ToString();
        _scoreMultiplier.text = "x" + str;
    }
    public void SetMultiplierSlider(float value)
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);
        _multiplierSlider.value = value;
    }
    #endregion
}
