using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ContactDevelper : MonoBehaviour
{
    private readonly string ClipBoardName = "hssong971221@gmail.com";
    public TextMeshProUGUI copyText;

    public TMP_InputField inputField;
    private string noAdsText;

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }
    public static void CopyToClipBoard(string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }

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


    public void OnEndEdit(string text)
    {
        //Debug.Log(text);
        noAdsText = text;

        if (noAdsText.Equals("NOADS"))
        {
            GameManager.Instance.isAdsPurchase = true;
            GameManager.Instance.SaveData();
        }
    }
}
