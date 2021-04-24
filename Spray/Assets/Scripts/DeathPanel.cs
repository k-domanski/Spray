using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image panel;

    private void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        text.CrossFadeAlpha(0f, 0.001f, true);
        panel.CrossFadeAlpha(0f, 0.001f, true);
    }

    public void Show()
    {
        text.CrossFadeAlpha(1f, 1.5f, true);
        panel.CrossFadeAlpha(1f, 1.5f, true);
    }
}