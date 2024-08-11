using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<GameManager>();
                singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TempAction();
        }
    }*/

    public enum Language
    {
        Kor,
        Eng,
        China,
    }
    public Language language;

    public static Action CheckProgAction; 

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CheckProgAction += CheckProgress;
    }

    // 호감도 시스템이랑 연동 될 곳
    public void CheckProgress()
    {
        //Debug.Log("progress check");
    }

    public void SetLanguage(Language language)
    {
        this.language = language;
    }

    public string GetText(Dictionary<string , string> data)
    {
        string selecter;
        switch (language)
        {
            case Language.Kor:
                selecter = "text";
                break;
            case Language.Eng:
                selecter = "entext";
                break;
            case Language.China:
                selecter = "china";
                break;
            default:
                selecter = "text";
                break;
        }
        if (data.TryGetValue(selecter, out var result))
            return result;
        else
            return "...";
    }

    
}
