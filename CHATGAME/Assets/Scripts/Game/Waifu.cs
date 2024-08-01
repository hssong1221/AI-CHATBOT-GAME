using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int affection_exp;/*호감도 경험치*/
    public int affection_lv;//호감도 레벨
    private int[] affection_barrel;//*호감도 레벨업 필요 경험치*/
    public string[] affection_status;//호감도 상태( intruder, suspicious, member, intimate, more, boyfriend )
    public string affection_restore;//엑셀에서 받아온 호감도를 저장
    DataManager dataManager;
    SheetData affSheet;

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
        affSheet = dataManager.GetSheetData("Dialogue");

        aff_idx = 0;

        affection_barrel = new int[] { Affection_sheet(0, "Poke") + Affection_sheet(0, "Event"), Affection_sheet(1, "Poke") + Affection_sheet(1, "Event"), Affection_sheet(2, "Poke") + Affection_sheet(2, "Event"), Affection_sheet(3, "Poke") + Affection_sheet(3, "Poke"), Affection_sheet(4, "Poke") + Affection_sheet(4, "Event"), Affection_sheet(5, "Poke") + Affection_sheet(5, "Event") };
        affection_status = new string[] { "Intruder", "Suspicious", "Member", "Intimate", "More", "Boyfriend" };

        Affection_compare();
    }

    public void Affection_ascend()
    {
        affection_exp++;
        Affection_level_calculate();
        Affection_compare();
    }

    public void Affection_descend()
    {
        if( affection_exp > 0 )
        {
            affection_exp--; 
        }
        Affection_level_calculate();
        Affection_compare();
    }

    public void Affection_level_calculate()
    {
        if(affection_exp >= affection_barrel[affection_lv])
        {
            affection_lv++;
            affection_exp = 0;
        }
    }

    public string Affection_transport()//UI로 호감도 수치를 전달함
    {
        return affection_lv.ToString();
    }

    public string Affection_compare()
    {
/*        if (affSheet == null)
            return "ERROR";

        var data = affSheet.GetData(_aff_idx);

        if(data == null )
        {
            return "empty";//임시
        }

        if(data.TryGetValue("affection",out var aff))//excel 파일에서 호감도 경로를 불러와 비교함
        {
            affection_restore = aff.ToString();
        }

        
        if (affection_lv <= int.Parse(affection_restore) )
        {
            //intruder
            affection_status = "Intruder";
        }
        else if (affection_lv > int.Parse(affection_restore) && affection_lv <= int.Parse(affection_restore) + 1)
        {
            //suspicious
            affection_status = "Suspicious";
        }
        else if (affection_lv > int.Parse(affection_restore) + 1 && affection_lv <= int.Parse(affection_restore) + 2)
        {
            //member
            affection_status = "Member";
        }
        else if(affection_lv > int.Parse(affection_restore) + 2 && affection_lv <= int.Parse(affection_restore) + 3)
        {
            //intimate
            affection_status = "Intimate";
        }
        else if (affection_lv > int.Parse(affection_restore) + 3 && affection_lv <= int.Parse(affection_restore) + 5)
        {
            //more
            affection_status = "More";
        }
        else if (affection_lv > int.Parse(affection_restore) + 5 && affection_lv <= int.Parse(affection_restore) + 8)
        {
            //boyfriend
            affection_status = "Boyfriend";
        }*/

        return affection_status[affection_lv];
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
