using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LogoBtnAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button btn;
    public TextMeshProUGUI btnText;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnText.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnText.color = Color.white;
    }
}
