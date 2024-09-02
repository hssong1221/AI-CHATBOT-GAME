using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using System;
public struct Item_Info
{
    //public Image mainImg;
    public string mainText;
}

public class UI_GalleryPanel : BasePanel, IRecyclableScrollRectDataSource
{
    #region Values
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    private List<Item_Info> _contactList = new List<Item_Info>();
    public List<int> affection_barrel = new List<int>();
    public List<string> date_img_id = new List<string>();

    public enum Category_status
    {
        Poke,
        Twitter,
        Pat,
        Date
    }
    public Category_status category_status;

    #endregion

    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        BarrelInit();
        category_status = Category_status.Poke;
    }

    public override void InitChild()
    {
        gameObject.SetActive(true);
        InitData();
    }

    #region Make Image Path
    public void BarrelInit()
    {
        int _cnt = 0;

        while (_cnt < 6)
        {
            affection_barrel.Add(Interact_cnt(_cnt, "Poke"));
            affection_barrel.Add(Interact_cnt(_cnt, "Event"));
            _cnt++;
        }
        PokeEventDataLength();
    }

    public void PokeEventDataLength()
    {
        int _cnt = 0;
        _dataLength = 0;
        while (_cnt < 6)
        {
            _dataLength += affection_barrel[_cnt];
            _cnt++;
        }
    }

    public string CheckCategory(int idx)
    {
        if (Waifu.Instance.dialogueData.Count <= 0)
        {
            return "Error";
        }

        //var poke_event_data = poke_event[idx];
        var poke_event_data = Waifu.Instance.dialogueData[idx];

        if (poke_event_data.TryGetValue("category", out var category))
        {
            return category.ToString();
        }
        else
        {
            return "Error";
        }
    }

    public string CheckAffectionLevel(int idx)
    {
        if (Waifu.Instance.dialogueData.Count <= 0/* || twitter.Count <= 0 || pat.Count <= 0 || date.Count <= 0*/)
        {
            return "Error";
        }

        List<string> aff_str = new List<string>() { "Intruder", "Suspicious", "Member", "Intimate", "More", "Boyfriend"};

        var poke_event_data = Waifu.Instance.dialogueData[idx];

        if (poke_event_data.TryGetValue("affection", out var affection))
        {
            return aff_str[int.Parse((affection.ToString()))];
        }
        else
        {
            return "Error";
        }
    }

    public int Interact_cnt(int aff_lv, string cate)
    {
        int _cnt = 0;
        List<Dictionary<string, string>> _data = new List<Dictionary<string, string>>();

        if(cate == "Poke" || cate == "Event")
        {
            _data = Waifu.Instance.dialogueData;
        }
        else if (cate == "Twt")
        {
            _data = AffectionTwt.Instance.twtData;
        }
        else if(cate == "Pat")
        {
            _data = AffectionPat.Instance.patData;
        }
        else if(cate == "Date")
        {
            _data = AffectionDate.Instance.dateData;
            var itr = _data.GetEnumerator();
            date_img_id.Clear();
            string temp = _data[0]["image_id"];
            date_img_id.Add(temp);
            _cnt = 1;

            while (itr.MoveNext())
            {
                var cur = itr.Current;

                if (cur["affection"].Equals(aff_lv.ToString()) && cur["category"].Equals(cate) && !cur["image_id"].Equals(temp))
                {
                    temp = cur["image_id"];
                    date_img_id.Add(temp);
                    _cnt++;
                }
            }
            return _cnt;
        }

        var iter = _data.GetEnumerator();
        while (iter.MoveNext())
        {
            var cur = iter.Current;

            if (cur["affection"].Equals(aff_lv.ToString()) && cur["category"].Equals(cate))
            {
                _cnt++;
            }
        }
        return _cnt;
    }

    public int CalculateCorrection(int idx)
    {
        int restore = 0;
        int _cnt = 0;
        
        while(idx >= 0)
        {
            restore = idx;
            idx -= affection_barrel[_cnt];
            if(idx < 0)
            {
                return restore;
            }
            _cnt++;
        }

        return restore;
    }

    public string CombineImgPath(int index)
    {
        string _combineImgPath = "";

        if(category_status.ToString() == "Poke")
        {
            _combineImgPath = $"image/{CheckCategory(index)}/{CheckAffectionLevel(index)}/{CalculateCorrection(index) + 1}";
        }
        else if(category_status.ToString() == "Twitter" || category_status.ToString() == "Pat")
        {
            _combineImgPath = $"image/{category_status.ToString()}/{index + 1}";
        }
        else if(category_status.ToString() == "Date")
        {
            _combineImgPath = $"image/{category_status.ToString()}/{date_img_id[index/* % date_img_id.Count*/]}";//юс╫ц
        }

        return _combineImgPath;
    }
    #endregion

    #region Recycle scroll
    //Initialising _contactList with dummy data 
    private void InitData()
    {
        if (_contactList != null) _contactList.Clear();

        for (int i = 0; i < _dataLength; i++)
        {
            Item_Info obj = new Item_Info();
            obj.mainText = i.ToString();
            _contactList.Add(obj);
        }
    }

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _contactList.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //string imgpath = $"image/{CheckCategory(index)}/{CheckAffectionLevel(index)}/{CalculateCorrection(index) + 1}";
        string imgpath = CombineImgPath(index);
        //Casting to the implemented Cell
        var item = cell as Item_GalleryScroll;
        item.ConfigureCell(_contactList[index], index, imgpath);
    }



    #endregion

    #region Buttons
    public void SetCategoryPokeBtn()
    {
        _recyclableScrollRect.ReloadData();
        category_status = Category_status.Poke;
        PokeEventDataLength();
        InitData();
    }

    public void SetCategoryTwtBtn()
    {
        _recyclableScrollRect.ReloadData();
        category_status = Category_status.Twitter;
        _dataLength = Interact_cnt(2, "Twt");
        InitData();
    }

    public void SetCategoryPatBtn()
    {
        _recyclableScrollRect.ReloadData();
        category_status = Category_status.Pat;
        _dataLength = Interact_cnt(3, "Pat");
        InitData();
    }

    public void SetCategoryDateBtn()
    {
        _recyclableScrollRect.ReloadData();
        category_status = Category_status.Date;
        _dataLength = Interact_cnt(3, "Date");
        InitData();
    }
    #endregion
}
