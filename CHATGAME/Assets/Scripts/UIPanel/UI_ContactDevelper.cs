using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ContactDevelper : MonoBehaviour
{
    private readonly string ClipBoardName = "hssong971221@gmail.com";
    public TextMeshProUGUI copyText;

    public void OnclickBtn()
    {
        CopyToClipBoard(ClipBoardName);
        copyText.gameObject.SetActive(true);
        Invoke("waitFunc", 3f);
    }

    public void waitFunc()
    {
        copyText.gameObject.SetActive(false);
    }

    public static void CopyToClipBoard(string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }
}
