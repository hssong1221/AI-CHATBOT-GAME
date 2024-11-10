using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertPanel : BasePanel
{
    private float fadeDuration = 2f;
    public Image alertPanelImg;
    public TextMeshProUGUI alertText;

    public override void InitChild()
    {
        alertPanelImg.color = new Color(0f, 0f, 0f, 0.8f);
        alertText.color = new Color(1f, 1f, 1f);
        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
        float startAlphaImg = alertPanelImg.color.a;
        float startAlphaTxt = alertText.color.a;
        float timeElapsed = 0f;
        float restoreAlpha = 0f;
        float restoreBeta = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            restoreAlpha = Mathf.Lerp(startAlphaImg, 0f, timeElapsed / fadeDuration);
            restoreBeta = Mathf.Lerp(startAlphaTxt, 0f, timeElapsed / fadeDuration);

            Color currentImgColor = alertPanelImg.color;
            Color currentTxtColor = alertText.color;
            alertPanelImg.color = new Color(currentImgColor.r, currentImgColor.g, currentImgColor.b, restoreAlpha);
            alertText.color = new Color(currentTxtColor.r, currentTxtColor.g, currentTxtColor.b, restoreBeta);

            yield return null;
        }

        Color finalColorImg = alertPanelImg.color;
        Color finalTxtColor = alertText.color;
        alertPanelImg.color = new Color(finalColorImg.r, finalColorImg.g, finalColorImg.b, 0f);
        alertText.color = new Color(finalTxtColor.r, finalTxtColor.g, finalTxtColor.b, 0f);
        EndPanel();
    }
}
