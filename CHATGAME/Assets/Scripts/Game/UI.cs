using System.Collections;
using System.Collections.Generic;
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
        ImgSheet = dataManager.GetSheetData("ImgPath");

        nameText.text = "name";
        affectionText.text = "0";
        dialogueText.text = "dialogue";
    }


    public void SetUI()
    {
        var data = diaSheet.GetData(Idx);
        var pdata = ImgSheet.GetData(0);
        if (data == null)
            return;

        string imgPath = "";
        if(pdata.TryGetValue("id", out var path))
            imgPath = path;

        

        if(data.TryGetValue("id", out var id))
            nameText.text = id;

        if (data.TryGetValue("affection", out var aff))
            affectionText.text = aff.ToString();

        if (data.TryGetValue("text", out var txt))
            dialogueText.text = txt;

        Idx++;
    }

    public void OnClickNextBtn()
    {
        SetUI();
    }
}
