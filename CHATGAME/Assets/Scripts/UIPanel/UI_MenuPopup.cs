using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MenuPopup : BasePanel
{
    public Button sndBtn;
    public Button ctrlBtn;
    public Button accBtn;

    public GameObject sndPanel;
    public GameObject ctrlPanel;
    public GameObject accPanel;
    public List<GameObject> panelList; // inspector 에서 집어넣기

    public Item1_MenuPopup item1_popup;

    public override void InitChild()
    {
        sndPanel.SetActive(true);
        ctrlPanel.SetActive(false);
        accPanel.SetActive(false);
        LoadSetting();
    }


    #region BTN
    public void OnClickSndBtn()
    {
        BtnAction(0);
        LoadSetting();
    }

    public void OnClickCtrlBtn()
    {
        BtnAction(1);
    }

    public void OnClickAccBtn()
    {
        BtnAction(2);
    }

    public void BtnAction(int idx)
    {
        foreach (GameObject panel in panelList)
            panel.SetActive(false);

        panelList[idx].SetActive(true);
    }
    #endregion

    public void LoadSetting()
    {

        float[] temp = new float[3] { GameManager.Instance.soundManager.Audio.volume, 0, 0 };
        item1_popup.Init(temp);
    }
}
