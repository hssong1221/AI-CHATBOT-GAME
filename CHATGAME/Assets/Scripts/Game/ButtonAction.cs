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

    public int unlockNumber;

    public static Action CheckUnlockAction;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        CheckUnlockAction += CheckLockNumber;
    }

    // юс╫ц 
    public void CheckLockNumber()
    {
        if (Waifu.Instance.affection_lv >= unlockNumber)
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
