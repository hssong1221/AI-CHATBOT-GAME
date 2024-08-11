using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // 데이터 직접 사용 및 변경 절대 금지
    public int affection_exp;
    public int affection_lv;
    public List<int> affection_interact;

    public PlayerData()
    {
        affection_exp = 0;
        affection_lv = 0;
        affection_interact = new List<int>();
    }

    public PlayerData(int affection_exp, int affection_lv, List<int> affection_interact)
    {
        this.affection_exp = affection_exp;
        this.affection_lv = affection_lv;
        this.affection_interact = new List<int>(affection_interact);
    }
}
