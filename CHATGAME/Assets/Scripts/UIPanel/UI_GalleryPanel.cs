using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UI_GalleryPanel : BasePanel
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _itemCount = 100;
    [SerializeField] private int _bufferItems = 10;

    private LinkedList<GameObject> _ItemList = new LinkedList<GameObject>();
    private RectTransform _contentRect;
    private float _ItemHeight;
    private int _tmpfirstVisibleIndex;
    private int _poolSize;

    void Start()
    {
        _contentRect = _content.GetComponent<RectTransform>();

        _ItemHeight = _slotPrefab.GetComponent<RectTransform>().rect.height;

        float contentHeight = _ItemHeight * _itemCount;

        _poolSize = (int)(_scrollRect.GetComponent<RectTransform>().sizeDelta.y / _ItemHeight) + _bufferItems * 2;

        _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, contentHeight);

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject item = Instantiate(_slotPrefab, _content);
            _ItemList.AddLast(item);
        }

        SlotInit();

        _scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        float contentY = _contentRect.anchoredPosition.y;

        int firstVisibleIndex = Mathf.Max(0, Mathf.FloorToInt(contentY / _ItemHeight) - _bufferItems);

        if(_tmpfirstVisibleIndex != firstVisibleIndex)
        {
            int diffIndex = _tmpfirstVisibleIndex - firstVisibleIndex;

            if (diffIndex < 0)
            {
                for (int i = 0, cnt = Mathf.Abs(diffIndex); i < cnt; i++)
                {
                    GameObject item = _ItemList.First.Value;
                    item.SetActive(true);
                    _ItemList.RemoveFirst();
                    _ItemList.AddLast(item);
                    item.transform.localPosition = new Vector3(0, (-(_tmpfirstVisibleIndex + _poolSize + i) * _ItemHeight) - _ItemHeight * 0.5f, 0);
                    if (_itemCount < (firstVisibleIndex + _poolSize + i))
                    {
                        item.SetActive(false);
                    }
                    else
                    {
                        item.SetActive(true);
                    }
                }
            }
            else if (diffIndex > 0)
            {
                for (int i = 0, cnt = Mathf.Abs(diffIndex); i < cnt; i++)
                {
                    GameObject item = _ItemList.Last.Value;
                    item.SetActive(true);
                    _ItemList.RemoveLast();
                    _ItemList.AddFirst(item);
                    item.transform.localPosition = new Vector3(0, (-(firstVisibleIndex - i) * _ItemHeight) - _ItemHeight * 0.5f, 0);
                }
            }

            _tmpfirstVisibleIndex = firstVisibleIndex;
        }
    }

    private void SlotInit()
    {
        int i = 0;
        foreach (GameObject item in _ItemList)
        {
            item.transform.localPosition = new Vector3(0, (-i * _ItemHeight) - _ItemHeight * 0.5f, 0);
            //item.GetComponentInChildren<Text>().text = (i + 1).ToString();
            i++;
        }
    }

    public override void InitChild()
    {
        gameObject.SetActive(true);
    }
}
