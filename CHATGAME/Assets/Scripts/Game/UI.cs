using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    #region Values
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

    public Animator animator;

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
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public float fillSpeed = 1f;
    private float currentFill = 0f;
    private float targetFill = 0f;

    DataManager dataManager;
    ICategory waifu;

    int DateLimitNum;
    bool isDateOut;

    SheetData diaSheet;

    public enum CategoryState
    {
        Poke,
        Event,
        Twitter,
        Pat,
        Date,
        Next,
    }
    [Header("현재 카테고리 상태")]
    public CategoryState categoryState;
    CategoryState lastState;
    
    public enum TextUIState
    {
        Before,
        Typing,
        End
    }
    [Header("현재 Text UI 상태")]
    public TextUIState textState;

    private Action SettingAction;
    #endregion

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
        //waifu = SingletonManager.Instance.GetSingleton<Waifu>();
        //waifu = SingletonManager.Instance.GetSingleton<AffectionPokeEvent>();
        if(GameManager.Instance.isDate)
        {
            SetCategoryState(CategoryState.Date);
            waifu = SingletonManager.Instance.GetSingleton<AffectionDate>();
        }
        else
        {
            waifu = SingletonManager.Instance.GetSingleton<Waifu>();
        }

        diaSheet = dataManager.GetSheetData("Dialogue");
        //ImgSheet = dataManager.GetSheetData("ImgPath");

        nameText.text = "name";
        affectionText.text = "0";
        dialogueText.text = "dialogue";

        TextDelayTime = 0.05f;
        //DataSheetSetting(0, "Poke");

        DateLimitNum = 0;
        isDateOut = false;

        SettingAction += SetMainImg;
        SettingAction += SetGauge;
        SettingAction += SetText;

        StartCoroutine(Init());
    }

    void Update()
    {
        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * fillSpeed);
        guageImg.fillAmount = currentFill / 1;
    }

    IEnumerator Init()
    {
        if(GameManager.Instance.isDate)
        {
            yield return new WaitUntil(() => waifu.GetDataList(CategoryState.Date.ToString()).Count > 0);
        }
        else
        {
            yield return new WaitUntil(() => waifu.GetDataList(CategoryState.Poke.ToString()).Count > 0);
        }
        

        //SettingAction?.Invoke();

        //waifu.Affection_ascend();
        //waifu.Interaction_Path();

        SettingAction?.Invoke();

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

    public void ReLoad()
    {
        SettingAction?.Invoke();
    }

    public void SetMainImg()
    {
        string category = "";   // 버튼 종류 + event
        string affState = "";   // 호감도 상태
        int imgFileName = 0;         // 이미지 파일 이름
        
        category = categoryState.ToString();
        affState = waifu.Affection_compare();
        imgFileName = waifu.Interact_img_path();

        string imgPath = "";
        
        if(category.Equals("Event") || GameManager.Instance.affection_lv % 2 == 1)
        {
            imgPath = $"image/Event/{affState}/{imgFileName/* + 1*/}";
        }
        else if (category.Equals("Poke")/* || category.Equals("Event")*/)
            imgPath = $"image/{category}/{affState}/{imgFileName/* + 1*/}";
        else if(category.Equals("Date"))
            imgPath = $"image/{category}/{AffectionDate.Instance.Interact_date_path()}";
        else
            imgPath = $"image/{category}/{imgFileName + 1}";
        
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
        heart1.gameObject.SetActive(ratio < 0.5);
        heart2.gameObject.SetActive(0.5 <= ratio && ratio < 1);
        heart3.gameObject.SetActive(ratio >= 1);
        targetFill = ratio;
        //guageImg.fillAmount = ratio;
    }

    public void SetText()
    {
        // waifu aff_poke_event_idx 부분은 이제 categorystate 마다 다른 idx가 들어가게 바꿔야 함
        var Idx = waifu.Interact_txt_path();

        var data = waifu.GetDataList(categoryState.ToString())[Idx];
        if (data == null)
            return;

        nameText.text = "리코";

        var txt = GameManager.Instance.GetText(data);

        textState = TextUIState.Typing;
        saveLastText = txt;
        typingCoroutine = StartCoroutine(TypingEffect(txt));
    }

    // dialogue text 타이핑 효과
    IEnumerator TypingEffect(string txt)
    {
        for (int i = 0; i < txt.Length; i++)
        {
            dialogueText.text = txt.Substring(0, i + 1);
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
        if (categoryState == CategoryState.Date)
        {
            //GameManager.Instance.date_sequence++;
            OnClickDateBtn();
            return;
        }

        waifu = SingletonManager.Instance.GetSingleton<Waifu>();

        /*foreach (var a in Waifu.Instance.affection_barrel)
            Debug.Log(a);*/

        if (textState == TextUIState.Typing)
            StopTypingEffect();
        else
        {
            waifu.Affection_ascend();
            waifu.Interaction_Path();

            ShowAdvertisement();

            string temp = waifu.Check_Category();
            if (temp.Equals("Poke"))
                SetCategoryState(CategoryState.Poke);
            else if (temp.Equals("Event"))
                SetCategoryState(CategoryState.Event);

            bool stateChange = false;
            if ((categoryState == CategoryState.Poke && lastState == CategoryState.Event) ||
                (categoryState == CategoryState.Event && lastState == CategoryState.Poke))
                stateChange = true;

            lastState = categoryState;

            if(stateChange)
            {
                animator.SetTrigger("isFadeInOut");
                return;
            }

            // Date가 끝나는 상태일 때 
            if (isDateOut)
                animator.SetTrigger("isDateOut"); // animation event function으로 PokeBtnEvent와 연결되어있음
            else
                PokeBtnEvent();

            
        }
    }

    public void PokeBtnEvent()
    {
        SettingAction?.Invoke();

        GameManager.Instance.unlockBtnCnt["Twitter"]++;
        GameManager.Instance.unlockBtnCnt["Pat"]++;
        GameManager.Instance.unlockBtnCnt["Date"]++;

        ButtonAction.CheckUnlockAction?.Invoke();
        DateLimitNum = 0;
        isDateOut = false;

        GameManager.Instance.SaveData();
    }

    public void OnClickTwtBtn()
    {
        waifu = SingletonManager.Instance.GetSingleton<AffectionTwt>();

        if (textState == TextUIState.Typing)
            StopTypingEffect();
        else
        {
            string temp = waifu.Check_Category();
            if (temp.Equals("Twt"))
                SetCategoryState(CategoryState.Twitter);

            //SettingAction?.Invoke();

            waifu.Affection_ascend();
            waifu.Interaction_Path();

            SettingAction?.Invoke();

            GameManager.Instance.unlockBtnCnt["Twitter"]=0;

            ButtonAction.CheckUnlockAction?.Invoke();
            GameManager.Instance.SaveData();
        }
    }
    public void OnClickPatBtn()
    {
        waifu = SingletonManager.Instance.GetSingleton<AffectionPat>();

        if (textState == TextUIState.Typing)
        {
            StopTypingEffect();
        }
        else
        {
            string temp = waifu.Check_Category();
            if (temp.Equals("Pat"))
                SetCategoryState(CategoryState.Pat);

            //SettingAction?.Invoke();

            waifu.Affection_ascend();
            waifu.Interaction_Path();

            SettingAction?.Invoke();

            GameManager.Instance.unlockBtnCnt["Pat"]=0;

            ButtonAction.CheckUnlockAction?.Invoke();
            GameManager.Instance.SaveData();
        }        
    }
    public void OnClickDateBtn()
    {
        waifu = SingletonManager.Instance.GetSingleton<AffectionDate>();

        if (textState == TextUIState.Typing)
        {
            StopTypingEffect();
        }
        else
        {
            if (GameManager.Instance.isDate)
                DataBtnEvent();
            else
                animator.SetTrigger("isDateIn"); // animation event function으로 DataBtnEvent와 연결되어있음
        }
    }


    public void DataBtnEvent()
    {
        string temp = waifu.Check_Category();
        if (temp.Equals("Date"))
            SetCategoryState(CategoryState.Date);

        GameManager.Instance.date_sequence++;
        waifu.Interaction_Path();

        SettingAction?.Invoke();

        var dateIdx = AffectionDate.Instance.Check_Current_Date();
        var data = AffectionDate.Instance.Date_number;
        //Debug.Log($"before : {DateLimitNum}  {data[dateIdx.ToString()]}");
        Debug.Log($"before : {GameManager.Instance.date_sequence}  {data[dateIdx.ToString()]}");
        DateLimitNum += 1;

        waifu.Affection_ascend();

        //if (DateLimitNum == data[dateIdx.ToString()])
        if (GameManager.Instance.date_sequence >= data[dateIdx.ToString()])
        {
            SetCategoryState(CategoryState.Poke);
            DateLimitNum = 0;
            waifu.Sequence_Init();
            Debug.Log($"Date{dateIdx} 끝");
            isDateOut = true;
        }

        
        //waifu.Interaction_Path();

        //SettingAction?.Invoke();

        GameManager.Instance.unlockBtnCnt["Date"] = 0;

        ButtonAction.CheckUnlockAction?.Invoke();
        GameManager.Instance.SaveData();
    }

    public void OnClickGalBtn()
    {
        UICtrl.Instance.ShowPanel("image/UI/UI_GalleryPanel", transform);
    }


    // temp version
    public void OnClickMenuButton()
    {
        UICtrl.Instance.ShowPanel("image/UI/UI_MenuPopup", transform);
    }
    public void OnclickLanBtn()
    {
        if (GameManager.Instance.language == GameManager.Language.Kor)
            GameManager.Instance.SetLanguage(GameManager.Language.Eng);
        else if (GameManager.Instance.language == GameManager.Language.Eng)
            GameManager.Instance.SetLanguage(GameManager.Language.Kor);

        UI.Instance.ReLoad();
    }

    public void OnclickSaveBtn()
    {
        GameManager.Instance.SaveData();
    }

    #endregion

    public void SetCategoryState(CategoryState state)
    {
        // 상태 변경
        categoryState = state;
    }

    public void SetCategoryState(string state)
    {
        categoryState = (CategoryState)Enum.ToObject(typeof(CategoryState), state);
    }

    public void ShowAdvertisement()
    {
        Debug.Log($"전체 인덱스 : {GameManager.Instance.Correction_number + GameManager.Instance.affection_exp}");
        // 엔딩을 보기 전에만 유효한 거라서 엔딩 후에도 일관되게 작동할 뭔가가 필요함
        if ((GameManager.Instance.Correction_number + GameManager.Instance.affection_exp) % 3 == 0)
        {
            RewardedAdsAction.rewardedAdsAction();
            return;
        }
    }
}
