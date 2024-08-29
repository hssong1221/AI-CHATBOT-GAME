using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

public class Item_GalleryScroll : MonoBehaviour, ICell
{
    //UI
    public Text nameLabel;

    //Model
    private Item_Info _itemInfo;
    private int _itemIdx;

    public void ConfigureCell(Item_Info itemInfo, int itemIndex)
    {
        _itemIdx = itemIndex;
        _itemInfo = itemInfo;

        nameLabel.text = itemInfo.mainText;
    }
}
