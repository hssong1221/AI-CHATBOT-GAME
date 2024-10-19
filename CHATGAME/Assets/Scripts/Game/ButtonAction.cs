using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAction : MonoBehaviour
{
    #region Values
    public Button button;
    public GameObject enableBtn;
    public GameObject disableBtn;
    public int unlockLevel;/// <summary>
    /// 버튼 활성화 기준 레벨
    /// </summary>
    public int unlockCount;/// <summary>
    /// 버튼 활성화 기준 poke 카운트 횟수
    /// </summary>
    public string cateType;/// <summary>
    /// 버튼 카테고리 타입
    /// </summary>

    public static Action CheckUnlockAction;
    #endregion

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        CheckUnlockAction += CheckLockNumber;
    }

    public void CheckLockNumber()
    {
        if (GameManager.Instance.affection_lv >= unlockLevel && GameManager.Instance.unlockBtnCnt[cateType] >= unlockCount && !GameManager.Instance.isDate && GameManager.Instance.affection_lv % 2 == 0)
            EnableBtn();
        else
            DisableBtn();
    }

    public void EnableBtn()
    {
        enableBtn.SetActive(true);
        disableBtn.SetActive(false);
    }

    public void DisableBtn()
    {
        enableBtn.SetActive(false);
        disableBtn.SetActive(true);
    }
}
