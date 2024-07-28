using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetData
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

    public Dictionary<string, string> GetData(int id)
    {
        if (id > Data.Count - 1)
            return null;

        return Data[id];
    }

    public void PrintAllData()
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
