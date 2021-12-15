using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EJetSpeed
{
    Normal,
    Slow,
    Fast
}

public class PlayerVisuals : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldObject;

    [Header("Jetpack")]
    public ParticleSystem[] jetpackJets;

    public Color normalJetColor;
    public Color slowJetColor;
    public Color fastJetColor;

    public void ShowShield(bool show)
    {
        shieldObject.SetActive(show);
    }

    public void ChangeJetColor(EJetSpeed jetSpeed)
    {
        foreach (ParticleSystem ps in jetpackJets)
        {
            switch (jetSpeed)
            {
                case EJetSpeed.Normal:
                    ps.GetComponent<ParticleSystemRenderer>().material.color = normalJetColor;
                    ps.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", normalJetColor);
                    break;

                case EJetSpeed.Slow:
                    ps.GetComponent<ParticleSystemRenderer>().material.color = slowJetColor;
                    ps.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", slowJetColor);

                    break;

                case EJetSpeed.Fast:
                    ps.GetComponent<ParticleSystemRenderer>().material.color = fastJetColor;
                    ps.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", fastJetColor);

                    break;
            }
        }
    }
}
