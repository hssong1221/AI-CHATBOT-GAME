using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public Button BackBtn;

    void Start()
    {
        BackBtn.onClick.AddListener(OnClickBackBtn);
    }

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        gameObject.SetActive(true);
        InitChild();
    }

    public virtual void InitChild() { }
    public virtual void EndPanel()
    {
        gameObject.SetActive(false);
        UICtrl.Instance.HidePanel(gameObject);
    }

    public virtual void OnClickBackBtn()
    {
        EndPanel();
    }
}
