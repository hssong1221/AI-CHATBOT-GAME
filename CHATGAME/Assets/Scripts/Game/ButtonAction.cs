using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAction : MonoBehaviour
{
    public Button button;
    public GameObject enableBtn;
    public GameObject disableBtn;
    //------임시 선언------//


    public int unlockLevel;
    public int unlockCount;
    public string cateType;

    public static Action CheckUnlockAction;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        CheckUnlockAction += CheckLockNumber;

        //button.onClick.AddListener(CheckLockNumber);
    }

    // 임시 
    public void CheckLockNumber()
    {
        if (GameManager.Instance.affection_lv >= unlockLevel && GameManager.Instance.unlockBtnCnt[cateType] >= unlockCount)
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
