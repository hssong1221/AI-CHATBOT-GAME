using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    private static UI _instance;
    public static UI Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<UI>();
                singletonObject.name = typeof(UI).ToString() + " (Singleton)";
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }

    public Image mainImg;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI affectionText;
    public TextMeshProUGUI dialogueText;

    DataManager dataManager;
    SheetData diaSheet;
    SheetData ImgSheet;

    public int Idx = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UI;
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        dataManager = SingletonManager.Instance.GetSingleton<DataManager>();
        diaSheet = dataManager.GetSheetData("Dialogue");
        //ImgSheet = dataManager.GetSheetData("ImgPath");

        nameText.text = "name";
        affectionText.text = "0";
        dialogueText.text = "dialogue";

        DataSheetSetting(0, "Poke");

        GameManager.CheckProgAction?.Invoke();
        SetMainImg();
        SetText();
    }

    public void SetMainImg()
    {
        /* 
         * img sheet로 만들었을 때
        var pdata = ImgSheet.GetData(0);
        string imgPath = "";

        if (pdata.TryGetValue("id", out var path))
            imgPath = path;
        */

        var data = diaSheet.GetData(Idx);

        string category = "";   // 버튼 종류 + event
        string affState = "";   // 호감도 상태
        int number = 0;         // 텍스트 순서
        
        if (data.TryGetValue("category", out var cat))
            category = cat;

        // waifu의 affectioncompare 를 사용할 것
        if (data.TryGetValue("affection", out var aff))
            affState = aff.ToString();

       

        if (data.TryGetValue("number", out var num))
            number = int.Parse(num);


        string imgPath = $"image/{category}/Intruder/{number}";
        Sprite sprite = Resources.Load<Sprite>(imgPath);
        if (sprite != null)
            mainImg.sprite = sprite;
        else
            Debug.LogWarning("경로에 사진 없음");
    }


    public void SetText()
    {
        var data = diaSheet.GetData(Idx);
        if (data == null)
            return;

        if(data.TryGetValue("id", out var id))
            nameText.text = id;

        if (data.TryGetValue("affection", out var aff))
            affectionText.text = aff.ToString();

        if (data.TryGetValue("text", out var txt))
            dialogueText.text = txt;
    }

    public void OnClickPokeBtn()
    {
        GameManager.CheckProgAction?.Invoke();
        SetMainImg();
        SetText();
        Idx++;
    }

    public void OnClickTwtBtn()
    {
        SetMainImg();
        SetText();
    }

    public void OnClickPatBtn()
    {
        SetMainImg();
        SetText();
    }
        public void OnClickNextBtn()
    {
        SetMainImg();
        SetText();
    }


    /// <summary>
    /// 파라미터 받아서 알맞게 데이터시트 가공
    /// </summary>
    /// <param name="affection">호감도 상태</param>
    /// <param name="category">버튼 종류</param>
    /// <returns></returns>
    public List<Dictionary<string , string>> DataSheetSetting(int affection, string category)
    {
        List<Dictionary<string, string>> partialData = new List<Dictionary<string, string>>();

        var data = diaSheet.Data;
        var iter = data.GetEnumerator();
        while(iter.MoveNext())
        {
            var cur = iter.Current;
            /*
            var iter2 = cur.GetEnumerator();
            while(iter2.MoveNext())
            {
                var cur2 = iter2.Current.Value;
                Debug.Log($"{cur2}");
            }
            */
            if (cur["affection"].Equals(affection.ToString()) && cur["category"].Equals(category))
                partialData.Add(cur);
        }

        return partialData;
    }
}
