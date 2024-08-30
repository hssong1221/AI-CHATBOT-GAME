using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

public class Item_GalleryScroll : MonoBehaviour, ICell
{
    //UI
    public Text nameLabel;
    public Image mainImg;

    //Model
    private Item_Info _itemInfo;
    private int _itemIdx;
    public string ImgPath;

    public void ConfigureCell(Item_Info itemInfo, int itemIndex, string _imgpath)
    {
        _itemIdx = itemIndex;
        _itemInfo = itemInfo;
        ImgPath = _imgpath;

        //nameLabel.text = itemInfo.mainText;
        nameLabel.text = _imgpath;

        Sprite sprite = Resources.Load<Sprite>(_imgpath);
        if (sprite != null)
            mainImg.sprite = sprite;
        else
            Debug.LogWarning("경로에 사진 없음");
    }
}
