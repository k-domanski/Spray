using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    #region Properties
    [SerializeField] private ScoreUIProxy _scoreUIProxy;
    [SerializeField] private float _baseMultiplierTime = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _timeReduction = 0.1f;
    public int score { get => _score; private set => SetScore(value); }
    public int multiplier { get => _multiplier; private set => SetMultiplier(value); }
    #endregion

    #region Private
    private int _score = 0;
    private int _multiplier = 1;
    private float _durationRemaining = 0.0f;
    private float _currentDuration = 0.0f;
    private float timeCoeff => (1.0f - _timeReduction);
    #endregion

    #region Messages
    void Start()
    {
        Reset();
    }
    void Update()
    {
        if (multiplier == 1)
        {
            return;
        }
        _durationRemaining -= Time.deltaTime;
        if (_durationRemaining <= 0.0f)
        {
            _currentDuration = GetMultiplierDuration(multiplier);
            _durationRemaining = _currentDuration;
            multiplier -= 1;
        }
        _scoreUIProxy.Get().SetMultiplierSlider(_durationRemaining / _currentDuration);
    }
    #endregion

    #region Public
    public void SetScore(int value)
    {
        _score = value;
        _scoreUIProxy.Get().SetScoreValue(value);
    }
    public void SetMultiplier(int value)
    {
        _multiplier = value > 0 ? value : 1;
        _scoreUIProxy.Get().SetScoreMultiplier(value);
        _currentDuration = GetMultiplierDuration(multiplier);
        _durationRemaining = _currentDuration;
    }
    public void AddScore(int value, int multiplierIncrease = 1)
    {
        score += value * multiplier;
        multiplier += multiplierIncrease;
    }
    public void Reset()
    {
        score = 0;
        multiplier = 1;
        _durationRemaining = 0.0f;
        _currentDuration = 0.0f;
        _scoreUIProxy.Get().SetMultiplierSlider(0.0f);
    }
    #endregion

    #region Private Methods
    private float GetMultiplierDuration(int mul)
    {
        return Mathf.Pow(timeCoeff, mul - 1) * _baseMultiplierTime;
    }
    #endregion
}
