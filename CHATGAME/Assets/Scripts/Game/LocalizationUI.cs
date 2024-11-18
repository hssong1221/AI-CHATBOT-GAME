using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationUI : MonoBehaviour
{
    /// <summary>
    /// UI Text 엑셀 파일의 id 값을 넣으면 해당하는 행에서 맞는 언어설정을 확인후에 열 값을 가져옴
    /// TextMeshPro에 component를 직접 추가 해줘야 함
    /// </summary>
    public int id;
    private TextMeshProUGUI uiText;

    public static Action reloadAction;

    void Start()
    {
        if (id == 0)
            return;

        uiText = GetComponent<TextMeshProUGUI>();
        //GetTextHelper();
        reloadAction += GetTextHelper;
    }

    private void OnEnable()
    {
        reloadAction?.Invoke();
    }

    public void GetTextHelper()
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(GetText());
    }
    IEnumerator GetText()
    {
        yield return new WaitUntil(() => DataManager.Instance is not null);

        var sheet = DataManager.Instance.GetSheetData("UIText");
        var data = sheet.Data[id - 1];
        var local = SetLocal();

        data.TryGetValue(local, out var text);
        uiText.text = text;
    }

    public string SetLocal()
    {
        switch (GameManager.Instance.language)
        {
            case GameManager.Language.Kor:
                return "KO";
            case GameManager.Language.Eng:
                return "EN";
            case GameManager.Language.Japan:
                return "JP";
            case GameManager.Language.China:
                return "CN";
            default:
                return "KO";
        }
    }

    private void OnDestroy()
    {
        reloadAction -= GetTextHelper;
    }
}
