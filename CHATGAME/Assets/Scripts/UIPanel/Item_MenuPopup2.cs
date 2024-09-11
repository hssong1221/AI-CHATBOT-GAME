using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_MenuPopup2 : MonoBehaviour
{
    public Button KorBtn;
    public Button EngBtn;
    public Button CNBtn;
    public Button JPBtn;

    public Button SaveBtn;

    public void Init(int languageIDX)
    {
        KorBtn.onClick.RemoveAllListeners();
        EngBtn.onClick.RemoveAllListeners();
        CNBtn.onClick.RemoveAllListeners();
        JPBtn.onClick.RemoveAllListeners();
        SaveBtn.onClick.RemoveAllListeners();

        KorBtn.onClick.AddListener(OnClickKORBtn);
        EngBtn.onClick.AddListener(OnClickENGBtn);
        CNBtn.onClick.AddListener(OnClickCNBtn);
        JPBtn.onClick.AddListener(OnClickJPBtn);
        SaveBtn.onClick.AddListener(OnClickSaveBtn);

        switch(languageIDX)
        {
            case 1:
                OnClickKORBtn();
                break;
            case 2:
                OnClickENGBtn();
                break;
            case 3:
                OnClickCNBtn();
                break;
            case 4:
                OnClickJPBtn();
                break;
            default:
                OnClickKORBtn();
                break;
        }
    }

    public void OnClickKORBtn()
    {
        if (GameManager.Instance.language == GameManager.Language.Kor)
            return;
        GameManager.Instance.SetLanguage(GameManager.Language.Kor);
        ReloadText();
    }
    public void OnClickENGBtn()
    {
        if (GameManager.Instance.language == GameManager.Language.Eng)
            return;
        GameManager.Instance.SetLanguage(GameManager.Language.Eng);
        ReloadText();
    }
    public void OnClickCNBtn()
    {
        if (GameManager.Instance.language == GameManager.Language.China)
            return;
        GameManager.Instance.SetLanguage(GameManager.Language.China);
        ReloadText();
    }
    public void OnClickJPBtn()
    {
        if (GameManager.Instance.language == GameManager.Language.Japan)
            return;
        GameManager.Instance.SetLanguage(GameManager.Language.Japan);
        ReloadText();
    }

    public void ReloadText()
    {
        UI.Instance.ReLoad();
    }

    public void OnClickSaveBtn()
    {
        Data.LanguageOpt = (int)GameManager.Instance.language;
        //UI_MenuPopup.backAction?.Invoke();
    }
}
