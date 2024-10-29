using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public Image temp;
    public bool isPurchase = false;

    private void Update()
    {
        if (isPurchase)
            temp.gameObject.SetActive(true);
    }

    public void OnPurchaseComplete(bool isComplete)
    {
        isPurchase = isComplete;
        GameManager.Instance.isAdsPurchase = true;
        GameManager.Instance.SaveData();
    }

    public void OnPurchaseFailed()
    {
        if (GameManager.Instance.isAdsPurchase)
            return;

        GameManager.Instance.isAdsPurchase = false;
    }

    public void OnProductFetched()
    {

    }
    
}
