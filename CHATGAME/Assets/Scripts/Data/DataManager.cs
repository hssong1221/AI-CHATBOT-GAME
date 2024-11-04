using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
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
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private Dictionary<string, SheetData> sheetsData;
    bool isEOF;
    bool fileReadEnd;
    private List<string> excelFileNames = new List<string> { "dialogue.xlsx", "UI text.xlsx" };

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as DataManager;
            SingletonManager.Instance.RegisterSingleton(_instance);
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isEOF = false;
        fileReadEnd = false;
        sheetsData = new Dictionary<string, SheetData>();
        ReadExcelFile();
        //PrintSheetData();
    }

    private void ReadExcelFile()
    {
#if UNITY_EDITOR
        //string filePath = "./Assets/Data/dialogue.xlsx";
        foreach (string fileName in excelFileNames)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Excel", fileName);
            ExcelFileLoad(filePath);
        }
#elif !UNITY_EDITOR && UNITY_ANDROID
        StartCoroutine(StreamingAssetsLoad());
#endif
    }

    IEnumerator StreamingAssetsLoad()
    {
        foreach (string fileName in excelFileNames)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Excel", fileName);
            Debug.Log(filePath);
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("데이터 들어옴");
                var data = www.downloadHandler.data;
                ExcelFileLoad(data);
            }
            else
            {
                Debug.Log("데이터 안들어옴");
            }
            yield return null;
        }
    }

    // 유니티 에디터에서
    void ExcelFileLoad(string filePath)
    {
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                {
                    var sheetData = new SheetData(table.TableName);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = new Dictionary<string, string>();
                        // 컬럼명 변경
                        if (i == 0)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                                table.Columns[j].ColumnName = table.Rows[i][j].ToString();

                            continue;
                        }
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            string columnName = table.Columns[j].ColumnName;
                            string cellValue = table.Rows[i][j].ToString();

                            if (cellValue.Equals("EOF"))
                                isEOF = true;

                            row[columnName] = cellValue;
                        }
                        if (isEOF)
                        {
                            isEOF = false;
                            break;
                        }
                        sheetData.AddData(row);
                    }
                    sheetsData[table.TableName] = sheetData;
                }
            }
            CheckAllFilesLoaded();
        }
    }

    // 안드로이드 환경에서
    void ExcelFileLoad(byte[] fileData)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = new MemoryStream(fileData))
        {
            //ExcelReaderConfiguration config = new() { FallbackEncoding = Encoding.UTF8 };

            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                {
                    var sheetData = new SheetData(table.TableName);
                    //Debug.Log(table.TableName);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = new Dictionary<string, string>();
                        // 컬럼명 변경
                        if (i == 0)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                                table.Columns[j].ColumnName = table.Rows[i][j].ToString();

                            continue;
                        }
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            string columnName = table.Columns[j].ColumnName;
                            string cellValue = table.Rows[i][j].ToString();

                            if (cellValue.Equals("EOF"))
                                isEOF = true;

                            //Debug.Log($"{columnName} : {cellValue}");
                            row[columnName] = cellValue;
                        }
                        if (isEOF)
                        {
                            isEOF = false;
                            break;
                        }
                        sheetData.AddData(row);
                    }
                    sheetsData[table.TableName] = sheetData;
                }
            }
            CheckAllFilesLoaded();
        }
    }

    private int loadedFileCount = 0;
    public void CheckAllFilesLoaded()
    {
        loadedFileCount++;
        if(loadedFileCount == excelFileNames.Count)
        {
            Waifu.Instance.SheetLoadAction?.Invoke();
            fileReadEnd = true;
        }
    }

    public IEnumerator WaitDataLoading(Action action)
    {
        yield return new WaitUntil(() => fileReadEnd == true);
        action?.Invoke();
    }

    public Dictionary<string, SheetData> GetAllSheetData()
    {
        return sheetsData; 
    }

    public SheetData GetSheetData(string sheetName)
    {
        if (sheetsData == null)
            return null;

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
