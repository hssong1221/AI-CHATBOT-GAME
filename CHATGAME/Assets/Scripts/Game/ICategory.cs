using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public interface ICategory
{
    public int Interact_idx { get; set; }
    public int Correction_number { get; set; }

    //public Affection_status affection_status { get; set; }

    public Action SheetLoadAction { get; set; }

    #region EXCEL Data
    public void SetSheetData();
    public void SheetData_Categorize();
    public List<Dictionary<string, string>> GetDataList(string name);
    #endregion

    public void Affection_ascend();
    public string Check_Category();

    public string Affection_compare();
    public void Interaction_Path();

    public float Affection_Percentage();
}
