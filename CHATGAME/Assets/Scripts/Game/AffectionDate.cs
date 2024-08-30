using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AffectionDate : MonoBehaviour, ICategory
{
    #region Values
    private static AffectionDate _instance;

    public static AffectionDate Instance
    {
        get
        {
            return _instance;
        }
    }

    private int _interact_idx = 0;
    public int Interact_idx
    {
        get { return _interact_idx; }
        set { _interact_idx = value; }
    }
    public int Correction_number { get; set; }
    public List<int> affection_barrel = new List<int>();
    public Dictionary<string, int> affection_increase = new Dictionary<string, int>() { { "Poke", 1 }, { "Event", 1 }, { "Twitter", 2 }, { "Pat", 2 }, { "Date", 2 } };//category 종류별 제공 경험치
    public List<int> date_affection_increase = new List<int>();/// <summary>
    /// 각각 date situation 의 호감도 상승치
    /// </summary>
    List<Dictionary<string, string>> dialogueData = new List<Dictionary<string, string>>();
    public List<Dictionary<string, string>> dateData = new List<Dictionary<string, string>>();

    public Dictionary<string,int> Date_number = new Dictionary<string, int>();/// <summary>
    /// 각 date situation 이 종료되는 시점의 엑셀 인덱스
    /// </summary>

    GameManager gameManager;
    DataManager dataManager;
    SheetData affSheet;
    SheetData dateSheet;

    ICategory poke_event_correct;

    public Action SheetLoadAction { get; set; }
    #endregion

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as AffectionDate;
            SingletonManager.Instance.RegisterSingleton(_instance);
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }

        SheetLoadAction += SetSheetData;
    }

    void Start()
    {
        StartCoroutine(DataManager.Instance.WaitDataLoading(SheetLoadAction));
        
        Interact_idx = 0;
        gameManager = SingletonManager.Instance.GetSingleton<GameManager>();
    }

    #region EXCEL Data
    public void SetSheetData()
    {
        dataManager = SingletonManager.Instance.GetSingleton<DataManager>();
        affSheet = dataManager.GetSheetData("Dialogue");
        dateSheet = dataManager.GetSheetData("Date");

        SheetData_Categorize();
    }

    public void SheetData_Categorize()
    {
        var iter = affSheet.Data.GetEnumerator();
        var date_itr = dateSheet.Data.GetEnumerator();
        int _cnt = 0;
        while (iter.MoveNext())
        {
            var cur = iter.Current;
            
            if (cur["category"].Equals("Poke") || cur["category"].Equals("Event"))
            {
                dialogueData.Add(cur);
            }
        }

        while(date_itr.MoveNext())
        {
            var val = date_itr.Current;

            if (val["category"].Equals("Date"))
            {
                dateData.Add(val);
            }
            if (Date_number.ContainsKey(val["situation"]))
                Date_number[val["situation"]]++;
            else
                //Date_number[val["situation"]] = 1;
                Date_number[val["situation"]] = _cnt;
            _cnt++;
        }
        Interact_Init();
        Barrel_Init();
        Increase_Init();
    }

    public void Barrel_Init()
    {
        int _cnt = 0;

        while (_cnt < 6)
        {
            affection_barrel.Add(Affection_sheet(_cnt, "Poke") * affection_increase["Poke"]);
            affection_barrel.Add(Affection_sheet(_cnt, "Event") * affection_increase["Event"]);
            _cnt++;
        }
    }

    public void Increase_Init()//Date 상호작용마다 호감도 상승치를 결정
    {
        var situation_temp="";
        var iter = dateData.GetEnumerator();

        while (iter.MoveNext())
        {
            var cur = iter.Current;

            if (cur["situation"].Equals(situation_temp.ToString()))
            {
                date_affection_increase.Add(0);
            }
            else
            {
                situation_temp = cur["situation"];
                date_affection_increase.Add(affection_increase["Date"]);
            }
        }
    }

    public List<Dictionary<string, string>> GetDataList(string name)
    {
        return dateData;
    }

    #endregion

    public void Affection_ascend()
    {
        //gameManager.affection_exp += date_affection_increase[_interact_idx];
        gameManager.affection_exp += date_affection_increase[gameManager.date_sequence];
        Affection_level_calculate();
    }

    public void Affection_level_calculate()
    {
        poke_event_correct = SingletonManager.Instance.GetSingleton<Waifu>();

        if (gameManager.affection_exp >= affection_barrel[gameManager.affection_lv])
        {
            gameManager.Correction_number += affection_barrel[gameManager.affection_lv];
            gameManager.affection_lv++;
            gameManager.affection_exp = -1;
        }
    }

    public float Affection_Percentage()
    {
        float aff_percent = 0f;
        
        if(gameManager.affection_lv % 2 == 1)
        {
            aff_percent = 1.0f;
        }
        else
        {
            aff_percent = (float)gameManager.affection_exp < (float)affection_barrel[gameManager.affection_lv] ? (float)gameManager.affection_exp / (float)affection_barrel[gameManager.affection_lv] : 1f;
        }

        return aff_percent;
    }

    public string Affection_compare()
    {
        return "";
    }

    public int Affection_sheet(int _aff_level, string _category)
    {
        int _aff_sheet = 0;
        List<Dictionary<string, string>> _data = new List<Dictionary<string, string>>();

        if (_category == "Poke" || _category == "Event")
        {
            _data = dialogueData;
        }
        else if (_category == "Date")
        {
            _data = dateData;
        }

        var iter = _data.GetEnumerator();
        while (iter.MoveNext())
        {
            var cur = iter.Current;

            if (cur["affection"].Equals(_aff_level.ToString()) && cur["category"].Equals(_category))
            {
                _aff_sheet++;
            }
        }
        return _aff_sheet;
    }

    public string Check_Category()
    {
        if (dateData.Count == 0)
        {
            return "Error";
        }

        //var data = dateData[_interact_idx];
        var data = dateData[gameManager.date_sequence];

        if (data.TryGetValue("category", out var cate))
        {
            return cate.ToString();
        }
        else
        {
            return "Error";
        }
    }

    public void Interaction_Path()
    {
        if(gameManager.date_interact.Count <= 0)
        {
            Interact_Init();
        }

        if(!gameManager.isDate)//데이트 첫 진입
        {
            //_date_temp = gameManager.date_interact[UnityEngine.Random.Range(0, gameManager.date_interact.Count)];
            //gameManager.date_interact.Remove(_date_temp);
            gameManager.date_sequence = gameManager.date_interact[UnityEngine.Random.Range(0, gameManager.date_interact.Count)];
            gameManager.date_interact.Remove(gameManager.date_sequence);
            gameManager.isDate = true;
        }
        //_interact_idx = _date_temp + gameManager.date_sequence;        
        //_interact_idx = gameManager.date_sequence;
        Debug.Log("random date idx : " + gameManager.date_sequence);
        //gameManager.date_sequence++;
    }

    public void Interact_Init()
    {
        if (gameManager.date_interact.Count > 0)
        {
            return;
        }

        int _cnt = 0;
        /*
        while (_cnt < Affection_sheet(3, "Date"))
        {
            gameManager.date_interact.Add(_cnt);
            _cnt++;
        }
        */
        var situation_temp = "";
        var iter = dateData.GetEnumerator();

        while (iter.MoveNext())
        {
            var cur = iter.Current;

            if (!cur["situation"].Equals(situation_temp.ToString()))
            {
                situation_temp = cur["situation"];
                gameManager.date_interact.Add(_cnt);
            }
            _cnt++;
        }
    }

    public int Interact_img_path()
    {
        return -1;
    }

    public int Interact_txt_path()
    {
        return gameManager.date_sequence;
    }

    public string Interact_date_path()
    {
        if (dateData.Count == 0)
        {
            return "Error";
        }

        //var data = dateData[_interact_idx];
        var data = dateData[gameManager.date_sequence];

        if (data.TryGetValue("image_id", out var cate))
        {
            return cate.ToString();
        }
        else
        {
            return "Error";
        }
    }

    public int Check_Current_Date()
    {
        //var data = dateData[_interact_idx];
        var data = dateData[gameManager.date_sequence];
        if (data.TryGetValue("situation", out var num))
        {
            return int.Parse(num);
        }
        else
        {
            return -1;
        }
    }

    public void Sequence_Init()
    {
        gameManager.date_sequence = 0;
        gameManager.isDate = false;
    }
}
