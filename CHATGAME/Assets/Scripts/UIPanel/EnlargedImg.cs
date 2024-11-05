using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnlargedImg : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Values
    public Image mainimg;
    public Image backgroundimg;
    public float zoomSpeed = 0.1f;
    public float minScale = 0.5f;
    public float maxScale = 3.0f;
    public float originalScale = 1.0f;
    public Button hidebtn;
    public GameObject frontbtn;
    public GameObject backbtn;
    public GameObject quitbtn;
    private Vector3 initpos;
    public GameObject gallobj;
    private int cell_idx;
    private UI_GalleryPanel gallclass;
    #endregion
    
    void Start()
    {
        initpos = mainimg.transform.position;//이미지 드래그 전 초기 위치 저장
        if(mainimg.rectTransform.localScale.x<minScale)
        {
            mainimg.rectTransform.localScale=new Vector3(minScale, minScale,1);
        }
    }

    void Update()
    {
        if(Input.touchCount>=2)
        {
            OnTouchScroll();
        }
    }

    #region Drag action
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
    #endregion

    #region Enlarge action
    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        float newScale = mainimg.rectTransform.localScale.x + scrollDelta * zoomSpeed;

        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        mainimg.rectTransform.localScale = new Vector3(newScale, newScale, 1);
    }

    public void OnTouchScroll()
    {
        Touch touch_0 = Input.GetTouch(0);
        Touch touch_1 = Input.GetTouch(1);

        float currentDistance = Vector2.Distance(touch_0.position, touch_1.position);
        float previousDistance = Vector2.Distance(touch_0.position - touch_0.deltaPosition, touch_1.position - touch_1.deltaPosition);

        float differ = currentDistance - previousDistance;

        originalScale += differ * zoomSpeed * 0.01f;
        originalScale = Mathf.Clamp(originalScale, minScale, maxScale);

        mainimg.rectTransform.localScale = new Vector3(originalScale, originalScale, 1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
    #endregion

    #region Button action
    public void Initlarge(int cellcnt)
    {
        if(gallobj == null)
        {
            gallobj = GameObject.Find("UI_GalleryPanel(Clone)");
        }
        gallclass = gallobj.GetComponent<UI_GalleryPanel>();
        if (gallclass == null) 
        {
            Debug.Log("Cannot find UGP class");
        }
        cell_idx = cellcnt;
    }

    public void Hide()
    {
        quitbtn.SetActive(!quitbtn.activeSelf);
        frontbtn.SetActive(!frontbtn.activeSelf);
        backbtn.SetActive(!backbtn.activeSelf);
    }

    public void Dislarge()//확대 보기 종료
    {
        Color mainalpha = mainimg.color;
        Color bgalpha = backgroundimg.color;
        quitbtn.SetActive(false);
        frontbtn.SetActive(false);
        backbtn.SetActive(false);
        mainalpha.a = 0f;
        bgalpha.a = 0f;
        mainimg.color = mainalpha;
        backgroundimg.color = bgalpha;
        mainimg.transform.position = initpos;
        backgroundimg.rectTransform.SetAsFirstSibling();
    }

    public void Movefront()
    {
        int restore = cell_idx;
        while(cell_idx < gallclass._contactList.Count)
        {
            if (gallclass._contactList[cell_idx].isunlock && cell_idx != restore)
            {
                break;
            }
            cell_idx++;
        }
        if(cell_idx >= gallclass._contactList.Count)//not exist
        {
            cell_idx = restore;
        }
        mainimg.sprite = Resources.Load<Sprite>(gallclass._contactList[cell_idx].imgPath);
        mainimg.transform.position = initpos;
        //Debug.Log("move front" + cell_idx);
    }

    public void Moveback() 
    {
        int restore = cell_idx;
        while (cell_idx >= 0)
        {
            if (gallclass._contactList[cell_idx].isunlock && cell_idx != restore)
            {
                break;
            }
            cell_idx--;
        }
        if (cell_idx < 0)//not exist
        {
            cell_idx = restore;
        }
        mainimg.sprite = Resources.Load<Sprite>(gallclass._contactList[cell_idx].imgPath);
        mainimg.transform.position = initpos;
        //Debug.Log("move back" + cell_idx);
    }
    #endregion
}
