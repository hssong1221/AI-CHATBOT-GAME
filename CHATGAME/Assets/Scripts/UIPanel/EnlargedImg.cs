using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnlargedImg : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IScrollHandler
{
    public RectTransform imgRectTransform;
    public RectTransform parentRT;
    public float zoomSpeed = 0.1f;
    public float minScale = 1.0f;
    public float maxScale = 3.0f;
    public Button hidebtn;
    public GameObject quitbtn;

    // Start is called before the first frame update
    void Start()
    {
        if(imgRectTransform.localScale.x<minScale)
        {
            imgRectTransform.localScale=new Vector3(minScale,minScale,1);
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
        Vector3 pos = imgRectTransform.position;
        pos += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        pos.x = Mathf.Clamp(pos.x, imgRectTransform.rect.xMin, imgRectTransform.rect.xMax);
        pos.y = Mathf.Clamp(pos.y, imgRectTransform .rect.yMin, imgRectTransform .rect.yMax);
        imgRectTransform.position = pos;
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
        float newScale = imgRectTransform.localScale.x + scrollDelta * zoomSpeed;

        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        imgRectTransform.localScale = new Vector3(newScale, newScale, 1);
    }

    public void Hide()
    {
        quitbtn.SetActive(!quitbtn.activeSelf);
    }

    public void Dislarge()
    {
        parentRT.SetAsFirstSibling();
    }
}
