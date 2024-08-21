using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    public void OnClickNewBtn()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.LoadData();
        PreCalculateAffectionModule();
        SceneManager.LoadScene("Game");
    }

    public void OnClickLoadBtn()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickGalleryBtn()
    {
        Debug.Log("구현 안했음");
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
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
