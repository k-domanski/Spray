using System;
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
    public float enableDuration = 1.0f;
    public GameObject shieldObject;

    [Header("Healing Effect")]
    public ParticleSystem healingEffect;

    [Header("Jetpack")]
    public ParticleSystem[] jetpackJets;

    public Color normalJetColor;
    public Color slowJetColor;
    public Color fastJetColor;

    private int edgeIndex = Shader.PropertyToID("_Edge");
    private Coroutine shieldCoroutine = null;
    private bool shieldEnabled = false;
    private float shieldEdge = 0.0f;

    void Start()
    {
        ShowHealingEffect(false);

        Material mt = shieldObject.GetComponent<Renderer>().material;
        mt.SetFloat(edgeIndex, 0.0f);
    }

    public void ShowShield(bool show)
    {
        if (show == shieldEnabled)
        {
            return;
        }
        shieldEnabled = show;

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        Material mt = shieldObject.GetComponent<Renderer>().material;
        float startV = shieldEdge;
        float endV = show ? 1.0f : 0.0f;
        float time = enableDuration * Mathf.Abs(endV - shieldEdge);
        shieldCoroutine = StartCoroutine(LerpShieldOverTime(startV, endV, time, (val) =>
        {
            mt.SetFloat(edgeIndex, val);
            shieldEdge = val;
        }));
    }

    public void ShowHealingEffect(bool show)
    {
        var emission = healingEffect.emission;
        emission.enabled = show;
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
    IEnumerator LerpShieldOverTime(float startValue, float endValue, float time, System.Action<float> AssignFunction)
    {
        float elapsed = 0.0f;
        while (elapsed < time)
        {
            float value = Mathf.Lerp(startValue, endValue, elapsed / time);
            AssignFunction(value);
            elapsed += Time.deltaTime;
            yield return null;
        }
        AssignFunction(endValue);
        shieldCoroutine = null;
        yield return null;
    }
}

