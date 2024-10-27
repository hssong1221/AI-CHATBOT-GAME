using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public Image temp;
    public bool check = false;

    public void OnPurchaseComplete(bool isComplete)
    {
        check = isComplete;
        Debug.Log(isComplete);
    }

    private void Update()
    {
        if (check)
            temp.gameObject.SetActive(true);
           
    }
}
