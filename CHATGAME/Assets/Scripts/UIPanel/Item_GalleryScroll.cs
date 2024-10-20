using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;

public class Item_GalleryScroll : MonoBehaviour, ICell
{
    //UI
    public Text nameLabel;
    public Image mainImg;

    //Model
    private Item_Info _itemInfo;
    private int _itemIdx;
    //public string ImgPath;
    private string _category;
    private string _imgId;

    //Action
    public Button button;
    public GameObject enableBtn;
    public GameObject disableBtn;

    public void ConfigureCell(Item_Info itemInfo, int itemIndex, /*string _imgpath,*/ string category, string imgId)
    {
        _itemIdx = itemIndex;
        _itemInfo = itemInfo;
        //ImgPath = _imgpath;
        _category = category;
        _imgId = imgId;

        //nameLabel.text = itemInfo.mainText;
        //nameLabel.text = _imgpath;

        //Sprite sprite = Resources.Load<Sprite>(_imgpath);
        Sprite sprite = Resources.Load<Sprite>(_itemInfo.imgPath);
        if (sprite != null)
            mainImg.sprite = sprite;
        else
            Debug.LogWarning("경로에 사진 없음");

        CheckUnlockGallery();
    }

    public void CheckUnlockGallery()
    {
        int temp = 0;

        if(_category == "Date")
        {
            if (GameManager.Instance.date_gallery_idx.TryGetValue(_imgId, out temp))
            {
                if (temp == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
        }
        else if((_category == "Poke" /*&& GameManager.Instance.poke_event_gallery_idx[_itemIdx] == 1*/))
        {/*
            var str = Waifu.Instance.dialogueData[_itemIdx]["category"];
            var afflv = int.Parse(Waifu.Instance.dialogueData[_itemIdx]["affection"]);
            var col = int.Parse(Waifu.Instance.dialogueData[_itemIdx]["number"])-1;

            if (str == "Poke" && afflv < 2)
            {
                if (GameManager.Instance.poke_event_gallery_list[afflv * 2][col] == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
            else if(str == "Poke" && afflv >= 2)
            {
                if (GameManager.Instance.poke_event_gallery_list[4][col] == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
            else if(str == "Event" && afflv == 0)
            {
                if (GameManager.Instance.poke_event_gallery_list[1][col] == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
            else if (str == "Event" && afflv == 1)
            {
                if (GameManager.Instance.poke_event_gallery_list[3][col] == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }
            else if (str == "Event" && afflv >= 2)
            {
                if (GameManager.Instance.poke_event_gallery_list[afflv+3][col] == 1)
                {
                    EnableBtn();
                }
                else
                {
                    DisableBtn();
                }
            }*/
            int row = 0;
            int restore = _itemIdx;
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
                EnableBtn();
            }
            else
            {
                DisableBtn();
            }
        }
        else
        {
            if( (_category == "Twitter" && GameManager.Instance.twt_gallery_idx[_itemIdx] == 1) || (_category == "Pat" && GameManager.Instance.pat_gallery_idx[_itemIdx] == 1))
            {
                EnableBtn();
            }
            else
            {
                DisableBtn();
            }
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
}
