using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectionPat : MonoBehaviour
{
    private static AffectionPat _instance;

    public static AffectionPat Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<AffectionPat>();
                singletonObject.name = typeof(AffectionPat).ToString() + "(Sington)";
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
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
    List<Dictionary<string, string>> dialogueData = new List<Dictionary<string, string>>();
    List<Dictionary<string, string>> patData = new List<Dictionary<string, string>>();

    GameManager gameManager;
    DataManager dataManager;
    SheetData affSheet;

    public Action SheetLoadAction { get; set; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as AffectionPat;
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
        Interact_idx = 0;
        gameManager = SingletonManager.Instance.GetSingleton<GameManager>();

        int _cnt = 0;

        while (_cnt < 6)
        {
            //affection_barrel.Add(Affection_sheet(_cnt, "Poke") * affection_increase["Poke"]);
            //affection_barrel.Add(Affection_sheet(_cnt, "Event") * affection_increase["Event"]);
            _cnt++;
        }
    }

    #region EXCEL Data
    public void SetSheetData()
    {
        dataManager = SingletonManager.Instance.GetSingleton<DataManager>();
        affSheet = dataManager.GetSheetData("Dialogue");

        SheetData_Categorize();
    }

    public void SheetData_Categorize()
    {
        var iter = affSheet.Data.GetEnumerator();
        while (iter.MoveNext())
        {
            var cur = iter.Current;
            if (cur["category"].Equals("Pat"))
            {
                patData.Add(cur);
            }
            else if (cur["category"].Equals("Poke") || cur["category"].Equals("Event"))
            {
                dialogueData.Add(cur);
            }

        }
    }

    public List<Dictionary<string, string>> GetDataList(string name)
    {
        return patData;
    }

    #endregion


}
