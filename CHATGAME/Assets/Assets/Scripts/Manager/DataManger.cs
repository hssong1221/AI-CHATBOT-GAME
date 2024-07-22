using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.IO;
using ExcelDataReader;

public class DataManger : MonoBehaviour
{
    private static DataManger _instance;
    public static DataManger Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<DataManger>();
                singletonObject.name = typeof(DataManger).ToString() + " (Singleton)";
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as DataManger;
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ReadExcelFile();
    }

    private void ReadExcelFile()
    {
        string filePath = "./Assets/Data/dialogue.xlsx";
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

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
            }
        }

    }
}
