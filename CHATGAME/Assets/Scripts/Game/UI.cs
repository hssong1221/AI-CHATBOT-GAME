using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    private static UI _instance;
    public static UI Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<UI>();
                singletonObject.name = typeof(UI).ToString() + " (Singleton)";
                SingletonManager.Instance.RegisterSingleton(_instance);
            }
            return _instance;
        }
    }

    [Header("텍스트 박스 UI")]
    public Image mainImg;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI affectionText;
    public TextMeshProUGUI dialogueText;
    [SerializeField]
    float TextDelayTime;
    Coroutine typingCoroutine;
    string saveLastText;

    [Header("호감도 Gauge")]
    public Image guageImg;

    DataManager dataManager;
    Waifu waifu;

    SheetData diaSheet;
    //SheetData ImgSheet;

    public enum CategoryState
    {
        Poke,
        Event,
        Twitter,
        Pat,
        Next,
    }
    [Header("현재 카테고리 상태")]
    public CategoryState categoryState;
    
    public enum TextUIState
    {
        Before,
        Typing,
        End
    }
    [Header("현재 Text UI 상태")]
    public TextUIState textState;

    private Action SettingAction;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as UI;
            SingletonManager.Instance.RegisterSingleton(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dataManager = SingletonManager.Instance.GetSingleton<DataManager>();
        waifu = SingletonManager.Instance.GetSingleton<Waifu>();

        diaSheet = dataManager.GetSheetData("Dialogue");
        //ImgSheet = dataManager.GetSheetData("ImgPath");

        nameText.text = "name";
        affectionText.text = "0";
        dialogueText.text = "dialogue";

        TextDelayTime = 0.05f;
        //DataSheetSetting(0, "Poke");

        GameManager.CheckProgAction?.Invoke();

        SettingAction += SetMainImg;
        SettingAction += SetGauge;
        SettingAction += SetText;

        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitUntil(() => waifu.GetDataList(CategoryState.Poke.ToString()).Count > 0);

        SettingAction?.Invoke();

        waifu.Affection_ascend();
        waifu.aff_idx += 1;

        yield return null;
    }

    /*void Update()
    {

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (isBtn)
                return;

            if (touch.phase == TouchPhase.Began)
                isBtn = true;
            else if (touch.phase == TouchPhase.Ended)
                isBtn = false;

            OnClickPokeBtn();
        }

    }*/

    #region UI data setting

    public void SetMainImg()
    {
        string category = "";   // 버튼 종류 + event
        string affState = "";   // 호감도 상태
        int imgFileName = 0;         // 이미지 파일 이름

        category = categoryState.ToString();
        affState = waifu.Affection_compare();
        imgFileName = waifu.affection_exp;

        string imgPath = $"image/{category}/{affState}/{imgFileName + 1}";
        Debug.Log($"현재 이미지 경로 : {imgPath}");

        Sprite sprite = Resources.Load<Sprite>(imgPath);
        if (sprite != null)
            mainImg.sprite = sprite;
        else
            Debug.LogWarning("경로에 사진 없음");
    }

    public void SetGauge()
    {
        float ratio = waifu.Affection_Percentage();
        guageImg.fillAmount = ratio;
    }

    public void SetText()
    {
        // waifu aff_idx 부분은 이제 categorystate 마다 다른 idx가 들어가게 바꿔야 함
        var Idx = waifu.aff_idx;

        var data = waifu.GetDataList(categoryState.ToString())[Idx];
        if (data == null)
            return;

        nameText.text = "리코 (이모티콘)";

        if (data.TryGetValue("text", out var txt))
        {
            //dialogueText.text = txt;
            textState = TextUIState.Typing;
            saveLastText = txt;
            typingCoroutine = StartCoroutine(TypingEffect(txt));
        }
    }

    // dialogue text 타이핑 효과
    IEnumerator TypingEffect(string txt)
    {
        for (int i = 0; i < txt.Length; i++)
        {
            dialogueText.text = txt.Substring(0, i);
            yield return new WaitForSeconds(TextDelayTime);
        }
        textState = TextUIState.End;
    }
    // 타이핑 효과 멈추고 즉시 전체 노출
    public void StopTypingEffect()
    {
        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        dialogueText.text = saveLastText;
        textState = TextUIState.End;
    }

    

    #endregion

    #region 버튼들

    public void OnClickPokeBtn()
    {
        if(textState == TextUIState.Typing)
        {
            StopTypingEffect();
        }
        else
        {
            GameManager.CheckProgAction?.Invoke();

            string temp = waifu.Check_Category();

            if (temp.Equals("Poke"))
                SetCategoryState(CategoryState.Poke);
            else if (temp.Equals("Event"))
                SetCategoryState(CategoryState.Event);

            SettingAction?.Invoke();

            waifu.Affection_ascend();
            waifu.aff_idx += 1;

            ButtonAction.CheckUnlockAction?.Invoke();
        }
    }
    public void OnClickTwtBtn()
    {
        categoryState = CategoryState.Twitter;

        SettingAction?.Invoke();

        waifu.Affection_ascend();
        waifu.aff_idx += 1;
    }
    public void OnClickPatBtn()
    {
        categoryState = CategoryState.Pat;

        SettingAction?.Invoke();

        waifu.Affection_ascend();
        waifu.aff_idx += 1;
    }
    public void OnClickNextBtn()
    {
        //categoryState = CategoryState.Next;

        SettingAction?.Invoke();

        waifu.Affection_ascend();
        waifu.aff_idx += 1;
    }

    #endregion

    public void SetCategoryState(CategoryState state)
    {
        categoryState = state;
    }

    public void SetCategoryState(string state)
    {
        categoryState = (CategoryState)Enum.ToObject(typeof(CategoryState), state);
    }

    /// <summary>
    /// 파라미터 받아서 알맞게 데이터시트 가공
    /// </summary>
    /// <param name="affection">호감도 상태</param>
    /// <param name="category">버튼 종류</param>
    /// <returns></returns>
    public List<Dictionary<string , string>> DataSheetSetting(int affection, string category)
    {
        List<Dictionary<string, string>> partialData = new List<Dictionary<string, string>>();

        var data = diaSheet.Data;
        var iter = data.GetEnumerator();
        while(iter.MoveNext())
        {
            var cur = iter.Current;
            /*
            var iter2 = cur.GetEnumerator();
            while(iter2.MoveNext())
            {
                var cur2 = iter2.Current.Value;
                Debug.Log($"{cur2}");
            }
            */
            if (cur["affection"].Equals(affection.ToString()) && cur["category"].Equals(category))
                partialData.Add(cur);
        }

        return partialData;
    }
}
