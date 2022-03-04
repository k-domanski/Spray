using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image panel;
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    public void Hide()
    {
        //text.CrossFadeAlpha(0f, 0.001f, true);
        //panel.CrossFadeAlpha(0f, 0.001f, true);
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(1, 0, 1, false));
    }

    public void Show()
    {
        //text.CrossFadeAlpha(1f, 1.5f, true);
        //panel.CrossFadeAlpha(1f, 1.5f, true);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(0, 1, 1, true));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        Systems.sceneManager.LoadScene(GameScene.Preload);
    }

    private IEnumerator Fade(float begin, float end, float time, bool enable)
    {
        float elapsed = 0;
        float delta = end - begin;
        while (elapsed < time)
        {
            canvasGroup.alpha = begin + delta * (elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
        fadeCoroutine = null;

        canvasGroup.interactable = enable;
    }
}