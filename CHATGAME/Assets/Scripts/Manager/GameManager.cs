using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Newtonsoft.Json;

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

    public PlayerData playerData;

    public enum Language
    {
        Kor,
        Eng,
        China,
    }
    public Language language;

    #region Affection Logic Var

    public int affection_exp;//호감도 경험치
    public int affection_lv;//호감도 레벨
    public List<int> affection_interact = new List<int>();//상호작용 인덱스 저장
    public List<int> twt_interact = new List<int>();
    public List<int> pat_interact = new List<int>();

    #endregion


    void Awake()
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

        if (PlayerPrefs.HasKey("PlayerData"))
            LoadData();
        else
            playerData = new PlayerData();
    }

    void Start()
    {
        //SingletonManager.Instance.RegisterSingleton(AffectionTwt.Instance);
        //SingletonManager.Instance.RegisterSingleton(AffectionPat.Instance);
    }


    #region Language

    public void SetLanguage(Language language)
    {
        this.language = language;
    }

    public string GetText(Dictionary<string, string> data)
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

    #endregion

    #region Data SAVE LOAD

    public void SaveData(PlayerData data)
    {
        string json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    public PlayerData LoadData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "{}");
        playerData = JsonConvert.DeserializeObject<PlayerData>(json);
        return playerData;
    }

#if UNITY_EDITOR
    [MenuItem("MyTools/Delete PlayerPrefs")]
    private static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("EXECUTE");
    }
#endif

    #endregion

}
