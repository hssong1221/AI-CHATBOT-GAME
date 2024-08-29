using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
public struct Item_Info
{
    //public Image mainImg;
    public string mainText;
}

public class UI_GalleryPanel : BasePanel, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    private List<Item_Info> _contactList = new List<Item_Info>();

    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
    }

    
    public override void InitChild()
    {
        gameObject.SetActive(true);
        InitData();
    }


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

    #region DATA-SOURCE

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
        //Casting to the implemented Cell
        var item = cell as Item_GalleryScroll;
        item.ConfigureCell(_contactList[index], index);
    }

    #endregion
}
