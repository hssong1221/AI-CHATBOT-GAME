using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class LogoManager : MonoBehaviour
{
    public Canvas logocanvas;
    public static Canvas logocanvas2;

    public GameObject logoObj;
    public GameObject btnObj;

    private void Awake()
    {
        logocanvas2 = logocanvas;
    }
    void Start()
    {
        btnObj.SetActive(false);
        Invoke("SecDelay", 1f);
    }

    void SecDelay()
    {
        btnObj.SetActive(true);
    }

    public void OnClickNewBtn()
    {
        PlayerPrefs.DeleteKey("PlayerData");
        GameManager.Instance.LoadData();
        PreCalculateAffectionModule();

        Invoke("StartGame", 0.5f);
    }

    public void OnClickLoadBtn()
    {
        Invoke("StartGame", 0.5f);
    }

    public void OnClickGalleryBtn()
    {
        if (!PlayerPrefs.HasKey("PlayerData"))
        {
            UICtrl.Instance.ShowPanel("image/UI/UI_AlertPanel", logocanvas.transform);
            return;
        }

        UICtrl.Instance.ShowPanel("image/UI/UI_GalleryPanel", logocanvas.transform);
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // _interact 미리 계산 다시
    public void PreCalculateAffectionModule()
    {
        Waifu.Instance.Interact_Init();
        AffectionTwt.Instance.Interact_Init();
        AffectionPat.Instance.Interact_Init();
        AffectionDate.Instance.Interact_Init();
        Waifu.Instance.Gallery_Index_Init();
        AffectionTwt.Instance.Gallery_Index_Init();
        AffectionPat.Instance.Gallery_Index_Init();
        //AffectionDate.Instance.Gallery_Index_Init();
    }


    public void OnClickDel()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("ALL DELETE");
    }
}
