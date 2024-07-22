using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetData : MonoBehaviour
{
    public string SheetName;
    public List<Dictionary<string, string>> Data;

    public SheetData(string sheetName)
    {
        SheetName = sheetName;
        Data = new List<Dictionary<string, string>>();
    }

    public void AddData(Dictionary<string, string> datum)
    {
        Data.Add(datum);
    }

    public void PrintData()
    {
        foreach (var row in Data)
        {
            foreach (var cell in row)
            {
                Debug.Log($"{cell.Key}: {cell.Value}");
            }
        }
    }
}
