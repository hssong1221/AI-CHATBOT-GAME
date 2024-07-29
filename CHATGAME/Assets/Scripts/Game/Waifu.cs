using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static int affection;//singletone 에서 관리할 호감도 수치
    public static string affection_status;//singletone 에서 관리할 호감도 상태
    DataManager dataManager;
    SheetData affSheet;

    void Awake()
    {
        if( _Instance == null )
        {
            _Instance = this as Waifu;
            SingletonManager.Instance.RegisterSingleton(_Instance);
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
    }

    public void Affection_ascend()
    {
        affection++;
    }
    public void Affection_descend()
    {
        affection--;
    }

    public string Affection_transport()//UI로 호감도 수치를 전달함
    {
        return affection.ToString();
    }

    public void Affection_compare()
    {
        if (affection < 0)//excel 파일에서 호감도 경로를 불러와 비교함
        {
            //intruder
            affection_status = "intruder";
        }
        else if (affection == 0)
        {
            //member
            affection_status = "member";
        }
        else if (affection > 0)
        {
            //suspicious
            affection_status = "suspicious";
        }
    }
}
