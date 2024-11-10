using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    private static UICtrl _instance;
    public static UICtrl Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<UICtrl>();
                singletonObject.name = typeof(UICtrl).ToString() + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }

    public Dictionary<string, GameObject> panelInstance = new Dictionary<string, GameObject>();

    GameObject CurrentPanel;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UICtrl;
            DontDestroyOnLoad(gameObject);
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject ShowPanel(string path, Transform parent)
    {
        GameObject pgo = Resources.Load<GameObject>(path);
        if(pgo == null)
        {
            Debug.Log("Game Obj Load FAIL");
            return null;
        }

        if(panelInstance.ContainsKey(pgo.name))
        {
            //panelInstance[pgo.name].SetActive(true);
            var bp = panelInstance[pgo.name].GetComponent<BasePanel>();

            bp.transform.SetParent(parent, false);

            bp.Init();

            return panelInstance[pgo.name].gameObject;
        }
        else
        {
            CurrentPanel = Instantiate(pgo, parent);
            panelInstance.Add(pgo.name, CurrentPanel);
            return pgo;
        }
    }

    public void HidePanel(GameObject panel)
    {
        panel.transform.SetParent(transform, false);
    }

}
