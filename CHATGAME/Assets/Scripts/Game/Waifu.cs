using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 호감도 시스템 로직을 담당
/// </summary>
public class Waifu : MonoBehaviour
{
    private static Waifu _Instance;

    public static Waifu Instance
    {
        get 
        {
            if( _Instance == null )
            {
                GameObject singletonObject = new GameObject();
                _Instance = singletonObject.AddComponent<Waifu>();
                singletonObject.name = typeof( Waifu ).ToString() + " (Singleton)";
                SingletonManager.Instance.RegisterSingleton(_Instance);
            }
            return _Instance;
        }
    }


    private int _aff_idx = 0;//DialogueSheet 행 Idx
    public int aff_idx
    {
        get { return _aff_idx; }
        set { _aff_idx = value; }
    }

    public int affection_exp;//호감도 경험치
    public int affection_lv;//호감도 레벨
    public List<int> affection_barrel = new List<int>();//호감도 레벨업 필요 경험치

    // ------------------------------------------------------------- event 임시로 1로 바꿔놓음 보고 바꾸기 -----------------------------------------------
    public Dictionary<string, int> affection_increase = new Dictionary<string, int>() { { "Poke", 1 }, { "Event", 1 }, { "Twt", 2 }, { "Pat", 2 }, { "Date", 2 } };//category 종류별 제공 경험치
    
    public enum Affection_status
    {
        Intruder,
        Suspicious,
        Member,
        Intimate,
        More,
        Boyfriend
    }
    [Header("호감도 상태")]
    public Affection_status _affection_status;
    
    public string category_restore;
    DataManager dataManager;
    SheetData affSheet;

    public static Action SheetLoadAction;

    void Awake()
    {
        if( _Instance == null )
        {
            _Instance = this as Waifu;
            SingletonManager.Instance.RegisterSingleton(_Instance);
            DontDestroyOnLoad(_Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dataManager = SingletonManager.Instance.GetSingleton<DataManager>();
        SheetLoadAction += SetSheetData;
        affSheet = dataManager.GetSheetData("Dialogue");
        aff_idx = 0;
        int _cnt = 0;

        while ( _cnt < 6)
        {
            //affection_barrel.Add(Affection_sheet(_cnt, "Poke") * affection_increase["Poke"] + Affection_sheet(_cnt, "Event") * affection_increase["Event"]);
            affection_barrel.Add(Affection_sheet(_cnt, "Poke") * affection_increase["Poke"]);
            affection_barrel.Add(Affection_sheet(_cnt, "Event") * affection_increase["Event"]);
            _cnt++;
        }
    }

    public void SetSheetData()
    {
        affSheet = dataManager.GetSheetData("Dialogue");
    }

    public void Affection_ascend()
    {
        if (affSheet == null)
            return ;

        var data = affSheet.GetData(_aff_idx);

        if (data == null)
        {
            return ;
        }
        
        if (data.TryGetValue("category", out var cate))//- dialogue datasheet 클래스에서 호감도 경로를 불러와 비교함
        {
            category_restore = cate.ToString();
        }

        affection_exp += affection_increase[category_restore];
        
        Affection_level_calculate();
    }


    public void Affection_level_calculate()
    {
        if(affection_exp >= affection_barrel[affection_lv])
        {
            affection_lv++;
            affection_exp = 0;
        }
    }

    public string Affection_compare()
    {
        _affection_status = (Affection_status)Enum.ToObject(typeof(Affection_status), affection_lv/2);
        return _affection_status.ToString();
    }
    
    public float Affection_Percentage()//호감도 UI 경험치 배율 전달
    {
        string _cate_str = Check_Category();
        float aff_percent = 0;
        if(_cate_str == "Event")
        {
            aff_percent = 1.0f;
        }
        else
        {
            aff_percent = (float)affection_exp / (float)affection_barrel[affection_lv];
        }

        return aff_percent;
    }


    // -------------------------------- 임시로 만든 카테고리 확인 함수--------------------------------
    public string Check_Category()
    {
        var data = affSheet.GetData(_aff_idx);
        data.TryGetValue("category", out var cate);
        /*if (data.TryGetValue("category", out var cate))
        {
            if (cate.Equals("Event") && affection_exp >= affection_barrel[affection_lv])
            {
                affection_exp = 0;
            }
            return cate;
        }
        return "Error";*/
        return cate.ToString();
    }

    public int Affection_sheet(int _aff_level, string _category)
    {
        int _aff_sheet = 0;

        var data = affSheet.Data;
        var iter = data.GetEnumerator();
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
}
