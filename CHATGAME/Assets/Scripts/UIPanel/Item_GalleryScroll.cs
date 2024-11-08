using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

public class Item_GalleryScroll : MonoBehaviour, ICell
{
    #region Values
    //UI
    //public Text nameLabel;
    public Image mainImg;
    public Image enlargedImgBackground;
    public Image enlargedImgMain;

    //Model
    private Item_Info _itemInfo;
    //private int _itemIdx;
    //private string _category;
    //private string _imgId;

    //Action
    public GameObject enableBtn;
    public GameObject disableBtn;
    #endregion

    #region Scroll Cell
    public void ConfigureCell(Item_Info itemInfo /*, int itemIndex, string category, string imgId*/)//Cell 내용 채우기
    {
        //_itemIdx = itemIndex;
        //_category = category;
        //_imgId = imgId;
        _itemInfo = itemInfo;

        Sprite sprite = Resources.Load<Sprite>(_itemInfo.imgPath);
        if (sprite != null)
        {
            mainImg.sprite = sprite;
        }
        else
            Debug.LogWarning("경로에 사진 없음");

        CheckUnlockGallery();
    }

    public void CheckUnlockGallery()//cell 활성화 상태 판별
    {/*
        int temp = 0;

        if(_category == "Date")
        {
            if (GameManager.Instance.date_gallery_idx.TryGetValue(_imgId, out temp))
            {
                if (temp == 1)
                {
                    _itemInfo.isunlock = true;
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
        }
        else if((_category == "Poke"))
        {
            int row = 0;
            //int restore = _itemIdx;
            int restore = _itemInfo.cell_idx;
            var poke_event_data = GameManager.Instance.poke_event_gallery_list;
            
            while(row < poke_event_data.Count)
            {
                if(restore - poke_event_data[row].Count < 0)
                {
                    break;
                }
                else
                {
                    restore -= poke_event_data[row].Count;
                    row++;
                }
            }

            if (poke_event_data[row][restore] == 1)
            {
                _itemInfo.isunlock = true;
                EnableBtn();
            }
            else
            {
                DisableBtn();
            }
        }
        else
        {
            if( (_category == "Twitter" && GameManager.Instance.twt_gallery_idx[_itemInfo.cell_idx] == 1) || (_category == "Pat" && GameManager.Instance.pat_gallery_idx[_itemInfo.cell_idx] == 1))
            {
                _itemInfo.isunlock = true;
                EnableBtn();
            }
            else
            {
                DisableBtn();
            }
        }
        */
        if(_itemInfo.isunlock)
        {
            EnableBtn();
        }
        else
        {
            DisableBtn();
        }
    }

    public void EnableBtn()
    {
        enableBtn.SetActive(true);
        disableBtn.SetActive(false);
    }

    public void DisableBtn()
    {
        enableBtn.SetActive(false);
        disableBtn.SetActive(true);
    }

    #endregion

    #region Enlarge
    public void OnClickEnlargeImg()//갤러리 이미지 확대해서 보기
    {
        EnlargeImgSet();
        /*Color mainalpha = enlargedImgMain.color;
        Color bgalpha = enlargedImgBackground.color;
        Sprite sprite = Resources.Load<Sprite>(_itemInfo.imgPath);
        if (sprite != null)
        {
            mainalpha.a = 1f;
            bgalpha.a = 0.9f;
            enlargedImgMain.color = mainalpha;
            enlargedImgBackground.color = bgalpha;
            enlargedImgMain.sprite = sprite;
        }*/
    }

    public void EnlargeImgSet()//확대용 이미지 컴포넌트 관련 기능
    {
        UICtrl.Instance.ShowPanel("image/UI/UI_EnlargeImgBackground", UI.Instance.gameObject.transform);
        var enlargeImgPanel = GameObject.Find("UI_EnlargeImgBackground(Clone)");
        var enlargeMain = enlargeImgPanel.GetComponent<EnlargedImg>();
        enlargeMain.InitPath(_itemInfo.imgPath);

        /*GameObject canvasobj = GameObject.Find("Canvas");
        Image[] imglist = canvasobj.GetComponentsInChildren<Image>(true);
        EnlargedImg enlargedImg;

        foreach (var target in imglist)
        {
            if (target.name == "EnlargeImgBackground")
            {
                enlargedImgBackground = target;
            }
            else if (target.name == "EnlargeImgMain")
            {
                enlargedImgMain = target;
            }
        }
        RectTransform rectTransform = enlargedImgBackground.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
        enlargedImg = enlargedImgMain.GetComponentInParent<EnlargedImg>();
        if (enlargedImg != null)
        {
            enlargedImg.Initlarge(_itemInfo.cell_idx);
        }
        else
        {
            Debug.Log("Cannot find enlargedimg class");
        }*/
    }
    #endregion
}
