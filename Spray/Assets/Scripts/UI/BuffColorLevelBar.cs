using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class BuffColorLevelBar : MonoBehaviour
{
    [SerializeField] ColorType _colorType;
    [SerializeField] PlayerProxy _player;
    private ProgressBar _progressBar;
    private void Awake()
    {
        _progressBar = GetComponent<ProgressBar>(); 
    }

    private void Update()
    {
        _progressBar.value = _player.Get().colorLevels[_colorType] / _player.Get().maxColorLevel;
    }

}
