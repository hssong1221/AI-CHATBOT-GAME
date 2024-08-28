using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UI_GalleryPanel : BasePanel
{
    public override void InitChild()
    {
        gameObject.SetActive(true);
    }
}
