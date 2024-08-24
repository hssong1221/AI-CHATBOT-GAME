using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    public void OnClickNewBtn()
    {
        PlayerPrefs.DeleteAll();
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
        Debug.Log("구현 안했음");
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
    }
}
