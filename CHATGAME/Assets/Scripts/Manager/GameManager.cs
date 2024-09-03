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

    //public PlayerData playerData; 
    //public NonInitData nonInitData;
    
    public SoundManager soundManager;

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
    public int Correction_number;/// <summary>
    /// 이미지 경로를 Interact_idx 에서 보정하기 위한 값, 호감도 레벨이 오를때마다 이전 레벨까지 레벨업 필요 경험치의 총합
    /// </summary>
    public int Interact_idx;/// <summary>
                            /// 이미지 경로와 텍스트 경로 인덱스
                            /// </summary>
    public bool isDate;/// <summary>
    /// date 상태 저장
    /// </summary>
    public int date_sequence;/// <summary>
    /// random date situation 의 진행도
    /// </summary>
    public List<int> affection_interact = new List<int>();/// <summary>
    /// 남아있는 poke, event 상호작용 인덱스 리스트
    /// </summary>
    public List<int> twt_interact = new List<int>();/// <summary>
    /// 남아있는 Twitter 상호작용 인덱스 리스트
    /// </summary>
    public List<int> pat_interact = new List<int>();/// <summary>
    /// 남아있는 pat 상호작용 인덱스 리스트
    /// </summary>
    public List<int> date_interact = new List<int>();/// <summary>
    /// 남아있는 date 상호작용 situation 시작 인덱스 리스트
    /// </summary>
    public Dictionary<string,int> unlockBtnCnt = new Dictionary<string,int>() { { "Twitter", 0 }, { "Pat", 0 }, { "Date", 0 } };/// <summary>
                                                                                                                                /// 버튼 액션 활성화 비교
                                                                                                                                /// </summary>
    #endregion

    #region NON_INIT Var
    public string temp; // 임시 데이터, 지우고 원하는거로 바꾸기

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
    }

    void Start()
    {
        LoadData();

        /*if (PlayerPrefs.HasKey("PlayerData"))
           LoadData() 
        else
            playerData = new PlayerData();*/
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

    // GameManager에 있는 변수들로 클래스를 만듬
    // 저장이 필요한 변수들을 계속 추가해주면 된다.
    public class PlayerData
    {
        public int affection_exp = 0;//호감도 경험치
        public int affection_lv = 0;//호감도 레벨
        public int Correction_number = 0;
        public int Interact_idx = 0;
        public bool isDate = false;
        public int date_sequence = 0;
        public List<int> affection_interact = new List<int>();//상호작용 인덱스 저장
        public List<int> twt_interact = new List<int>();
        public List<int> pat_interact = new List<int>();
        public List<int> date_interact = new List<int>();
        public Dictionary<string, int> unlockBtnCnt = new Dictionary<string, int>() { { "Twitter", 0 }, { "Pat", 0 }, { "Date", 0 } };
    }
    public class NonInitData
    {
        public string temp;
    }

    // 데이터 저장을 위해 클래스안에 기존 데이터 주입
    public PlayerData GetPlayerData()
    {
        return new PlayerData
        {
            affection_exp = this.affection_exp,
            affection_lv = this.affection_lv,
            Correction_number = this.Correction_number,
            Interact_idx = this.Interact_idx,
            isDate = this.isDate,
            date_sequence = this.date_sequence,
            affection_interact = this.affection_interact,
            twt_interact = this.twt_interact,
            pat_interact = this.pat_interact,
            date_interact = this.date_interact,
            unlockBtnCnt = this.unlockBtnCnt,
        };
    }

    // 데이터 로드를 위해 클래스 안에 있는 데이터 꺼내서 GameManager 변수 초기화
    public void SetPlayerData(PlayerData data)
    {
        this.affection_exp = data.affection_exp;
        this.affection_lv = data.affection_lv;
        this.Correction_number = data.Correction_number;
        this.Interact_idx = data.Interact_idx;
        this.isDate = data.isDate;
        this.date_sequence = data.date_sequence;
        this.affection_interact = data.affection_interact;
        this.twt_interact = data.twt_interact;
        this.pat_interact = data.pat_interact;
        this.date_interact = data.date_interact;
        this.unlockBtnCnt = data.unlockBtnCnt;
    }

    public NonInitData GetNonInitData()
    {
        return new NonInitData
        {
            temp = this.temp,
        };
    }
    public void SetNonInitData(NonInitData data)
    {
        this.temp = data.temp;
    }

    // 데이터 저장을 하려면 부르시오
    public void SaveData()
    {
        PlayerData data = GetPlayerData();
        NonInitData data2 = GetNonInitData();
        string json = JsonConvert.SerializeObject(data);
        string json2 = JsonConvert.SerializeObject(data2);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.SetString("NonInitData", json2);
        PlayerPrefs.Save();
    }

    // 데이터 로드를 하려면 부르시오(겜 켜질 때 한번 부르면 될거 같음)
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey("PlayerData"))
        {
            PlayerData data = new PlayerData();
            SetPlayerData(data);
        }
        else
        {
            string json = PlayerPrefs.GetString("PlayerData", "{}");
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            SetPlayerData(playerData);
        }

        if(!PlayerPrefs.HasKey("NonInitData"))
        {
            NonInitData data2 = new NonInitData();
            SetNonInitData(data2);
        }
        else
        {
            string json2 = PlayerPrefs.GetString("NonInitData", "{}");
            NonInitData niData = JsonConvert.DeserializeObject<NonInitData>(json2);
            SetNonInitData(niData);
        }
    }

#if UNITY_EDITOR
    // 실험용 기능 - playerprefs에 저장된 데이터 날리는 함수
    [MenuItem("MyTools/Delete PlayerPrefs")]
    private static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("EXECUTE");
    }
#endif

    #endregion

}
