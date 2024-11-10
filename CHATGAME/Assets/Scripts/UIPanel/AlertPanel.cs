using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : BasePanel
{
    private float fadeDuration = 1f;
    public Image alertPanelImg;

    public override void InitChild()
    {
        alertPanelImg.color = new Color(1f, 1f, 1f, 0.5f);
        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
        float startAlpha = alertPanelImg.color.a;
        float timeElapsed = 0f;
        float restoreAlpha = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            restoreAlpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);

            Color currentColor = alertPanelImg.color;
            alertPanelImg.color = new Color(currentColor.r, currentColor.g, currentColor.b, restoreAlpha);
            yield return null;
        }

        Color finalColor = alertPanelImg.color;
        alertPanelImg.color = new Color(finalColor.r, finalColor.g, finalColor.b, 0f);
        EndPanel();
    }
}
