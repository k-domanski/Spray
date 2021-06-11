using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WaveNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentWaveNumber;
    public void updateWaveNumber(int waveNumber)
    {
        
        currentWaveNumber.text = "Current wave: " + waveNumber.ToString();
    }
}
