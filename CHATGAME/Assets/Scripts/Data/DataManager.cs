using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.IO;
using ExcelDataReader;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<DataManager>();
                singletonObject.name = typeof(DataManager).ToString() + " (Singleton)";
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }

    private Dictionary<string, SheetData> sheetsData;
    bool isEOF;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as DataManager;
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isEOF = false;
        ReadExcelFile();
        //PrintSheetData();
    }

    private void ReadExcelFile()
    {
        //string filePath = "./Assets/Data/dialogue.xlsx";
        string filePath = "../dialogue.xlsx";

        sheetsData = new Dictionary<string, SheetData>();

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                foreach(DataTable table in result.Tables)
                {
                    var sheetData = new SheetData(table.TableName);

                    for(int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = new Dictionary<string, string>();
                        // 컬럼명 변경
                        if(i == 0)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                                table.Columns[j].ColumnName = table.Rows[i][j].ToString();

                            continue;
                        }
                        for(int j = 0; j < table.Columns.Count; j++)
                        {
                            string columnName = table.Columns[j].ColumnName;
                            string cellValue = table.Rows[i][j].ToString();

                            if(cellValue.Equals("EOF"))
                                isEOF = true;

                            row[columnName] = cellValue;
                        }
                        if (isEOF)
                            break;
                        sheetData.AddData(row);
                    }
                    sheetsData[table.TableName] = sheetData;
                }
                
                /*
                // 첫 번째 시트의 데이터를 읽습니다.
                DataTable table = result.Tables[0];

                for (int i = 1; i < table.Rows.Count; i++) // 첫 번째 행은 헤더이므로 건너뜁니다.
                {
                    var column1 = table.Rows[i][0];

                    if (column1.Equals("EOF"))
                        break;
                    else
                        column1 = int.Parse(table.Rows[i][1].ToString());

                    int column2 = int.Parse(table.Rows[i][1].ToString());
                    string column3 = table.Rows[i][2].ToString();
                    int column4 = int.Parse(table.Rows[i][3].ToString());
                    string column5 = table.Rows[i][4].ToString();
                    Debug.Log($"Column1: {column1}, Column2: {column2}, Column3: {column3}, Column4: {column4}, Column5: {column5}" );
                }
                */
            }
        }
    }

    public Dictionary<string, SheetData> GetAllSheetData()
    {
        return sheetsData;
    }

    public SheetData GetSheetData(string sheetName)
    {
        if (sheetsData.TryGetValue(sheetName, out var result))
            return result;
        else
            return null;
    }

    public void PrintSheetData()
    {
        foreach (var sheet in sheetsData)
        {
            Debug.Log($"Sheet: {sheet.Key}");
            sheet.Value.PrintAllData();
        }
    }
}
