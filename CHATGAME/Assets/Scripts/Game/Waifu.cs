using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waifu : MonoBehaviour
{
    public static int affection;//singletone 에서 관리할 호감도 수치
    public static string affection_status;//singletone 에서 관리할 호감도 상태

    public void affection_ascend()
    {
        affection++;
    }
    public void affection_descend()
    {
        affection--;
    }

    public string affection_transport()//UI로 호감도 수치를 전달함
    {
        return affection.ToString();
    }

    public void affection_compare()
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
