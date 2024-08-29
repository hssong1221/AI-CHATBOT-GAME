using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotScroll : MonoBehaviour
{
    public int first;
    public int second;
    public int third;

    public void Init_Img_Path(int value)
    {
        first = value;
        second = value + 1;
        third = value + 2;
    }
}
