using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectionPokeEvent : Waifu
{
    private int _aff_poke_event_idx = 0;//DialogueSheet 행 Idx
    private int _interact_idx = 0;
    public int Interact_idx
    {
        get { return _interact_idx; }
        set { _interact_idx = value; }
    }

    private int _correction_number;//경로 위치 보정
    public int Correction_number
    {
        get { return _correction_number; }
        set { _correction_number = value; }
    }

    public List<int> affection_interact = new List<int>();//상호작용 인덱스 저장

    
    List<Dictionary<string, string>> dialogueData = new List<Dictionary<string, string>>();
    

    public static Action SheetLoadAction;
    public override void Affection_ascend()
    {
        affection_exp += affection_increase[category_restore];

        Affection_level_calculate();
    }

    public override void Affection_level_calculate()
    {
        int _cnt = 1;

        if (affection_exp >= affection_barrel[affection_lv])
        {
            _correction_number += affection_barrel[affection_lv];
            affection_lv++;
            affection_exp = 0;
            affection_interact.Clear();

            while (_cnt <= affection_barrel[affection_lv])
            {
                affection_interact.Add(_cnt);//임의의 대사 인덱스를 전달하기 위한 작업
                _cnt++;
            }
        }
    }
    
    public override void Interaction_Path()//Poke 상호작용 경로 번호 찾기
    {
        int _I_P_N = _correction_number;
        int _restore_rand = 0;
        category_restore = "Poke";

        if (affection_lv < 4)//호감도 상태가 Member 미만인 경우
        {
            _I_P_N += affection_exp;
        }
        else if (affection_lv % 2 == 1)//Event 인 경우
        {
            _I_P_N += affection_exp;
            category_restore = "Event";
        }
        else if (affection_lv >= 4 && affection_lv % 2 == 0)//호감도 상태가 Member 이상인 경우 임의의 중복되지 않는 대사 인덱스를 전달함
        {
            _restore_rand = affection_interact[UnityEngine.Random.Range(0, affection_interact.Count)];
            affection_interact.Remove(_restore_rand);
            _I_P_N += _restore_rand;
        }

        _aff_poke_event_idx = _I_P_N;
        Interact_compare();
    }

    public override void Interact_Init()
    {
        int _cnt = 0;

        while (_cnt < Affection_sheet(0, "Poke"))
        {
            affection_interact.Add(_cnt);
            _cnt++;
        }
    }
    /*
    private void Twt_Interaction_Init()
    {
        int _cnt = 0;

        while(_cnt < Affection_sheet(2,"Twt"))
        {
            twt_interact.Add(_cnt);
            _cnt += 1;
        }
    }
    
    public void Twt_Interaction_Path(string _aff_stat)
    {
        int _restore_rand = twt_interact[UnityEngine.Random.Range(0,twt_interact.Count)];
        twt_interact.Remove(_restore_rand);
        _aff_twt_idx = _restore_rand;
        category_restore=_aff_stat;
        Interact_compare();
    }

    private void Pat_Interaction_Init()
    {
        int _cnt = 0;

        while (_cnt < Affection_sheet(3, "Pat"))
        {
            twt_interact.Add(_cnt);
            _cnt += 1;
        }
    }

    public void Pat_Interaction_Path(string _aff_stat)
    {
        int _restore_rand = pat_interact[UnityEngine.Random.Range(0, pat_interact.Count)];
        pat_interact.Remove(_restore_rand);
        _aff_pat_idx = _restore_rand;
        category_restore = _aff_stat;
        Interact_compare();
    }
    */
    public override void Interact_compare()
    {
        if (category_restore == "Poke" || category_restore == "Event")
        {
            _interact_idx = _aff_poke_event_idx;
        }/*
        else if (category_restore == "Twitter")
        {
            _interact_idx = _aff_twt_idx;
        }
        else if(category_restore == "Pat")
        {
            _interact_idx = _aff_pat_idx;
        }*/
    }

    public override string Check_Category()//카테고리 확인
    {
        if (dialogueData.Count == 0)
            return "Error";
        //var data = dialogueData[_aff_poke_event_idx];

        var data = dialogueData[_aff_poke_event_idx];

        if (data.TryGetValue("category", out var cate))
        {
            return cate.ToString();
        }
        else
        {
            return "Error";
        }
    }

    public override int Affection_sheet(int _aff_level, string _category)//특정 호감도 레벨에서 특정 상호작용의 수
    {
        int _aff_sheet = 0;
        List<Dictionary<string, string>> _data = new List<Dictionary<string, string>>();

        if (_category == "Poke" || _category == "Event")
        {
            _data = dialogueData;
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



    public string Affection_compare()
    {
        affection_status = (Affection_status)Enum.ToObject(typeof(Affection_status), affection_lv / 2);
        return affection_status.ToString();
    }

    public float Affection_Percentage()//호감도 UI 경험치 배율 전달
    {
        string _cate_str = Check_Category();
        float aff_percent = 0;
        if (_cate_str == "Event")
        {
            aff_percent = 1.0f;
        }
        else
        {
            aff_percent = (float)affection_exp / (float)affection_barrel[affection_lv];
        }

        return aff_percent;
    }

}
