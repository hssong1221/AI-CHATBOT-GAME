using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnlargedImg : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IScrollHandler
{
    public Image mainimg;
    public Image backgroundimg;
    public float zoomSpeed = 0.1f;
    public float minScale = 1.0f;
    public float maxScale = 3.0f;
    public Button hidebtn;
    public GameObject quitbtn;
    private Vector3 initpos;

    // Start is called before the first frame update
    void Start()
    {
        initpos = mainimg.transform.position;
        if(mainimg.rectTransform.localScale.x<minScale)
        {
            mainimg.rectTransform.localScale=new Vector3(minScale, minScale,1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hidebtn != null) 
        {
            hidebtn.interactable = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localpos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainimg.rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localpos);
        mainimg.rectTransform.localPosition = localpos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hidebtn != null)
        {
            hidebtn.interactable = true;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        float newScale = mainimg.rectTransform.localScale.x + scrollDelta * zoomSpeed;

        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        mainimg.rectTransform.localScale = new Vector3(newScale, newScale, 1);
    }

    public void Hide()
    {
        quitbtn.SetActive(!quitbtn.activeSelf);
    }

    public void Dislarge()
    {
        Color mainalpha = mainimg.color;
        Color bgalpha = backgroundimg.color;
        quitbtn.SetActive(false);
        mainalpha.a = 0f;
        bgalpha.a = 0f;
        mainimg.color = mainalpha;
        backgroundimg.color = bgalpha;
        mainimg.transform.position = initpos;
        backgroundimg.rectTransform.SetAsFirstSibling();
    }
}
