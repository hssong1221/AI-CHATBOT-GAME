using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_MenuPopup1 : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;

    public Button SaveBtn;

    private void Start()
    {
        slider1.onValueChanged.AddListener(OnSlider1Changed);
    }

    public void Init(float[] valList)
    {
        slider1.value = valList[0];
        slider2.value = valList[1];
        slider3.value = valList[2];

        GameManager.Instance.soundManager.SoundSetting(slider1.value);

        SaveBtn.onClick.RemoveAllListeners();
        SaveBtn.onClick.AddListener(OnClickSaveBtn);
    }

    public void OnSlider1Changed(float val)
    {
        slider1.value = val;
        GameManager.Instance.soundManager.SoundSetting(val);
    }

    public void OnClickSaveBtn()
    {
        Data.SoundOpt = slider1.value;
        //UI_MenuPopup.backAction?.Invoke();
    }
}
