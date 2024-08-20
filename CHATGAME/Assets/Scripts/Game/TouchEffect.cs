using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform rt;
    public GameObject effect;

    public float limitTime = 0.05f;
    float TouchTime = 0f;

    public List<GameObject> touchObjectPool = new List<GameObject>();
    void Update()
    {
        if(Input.GetMouseButton(0) && TouchTime >= limitTime)
        {
            TouchTime = 0f;
            // 클릭 위치
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, Camera.main, out var localPoint);

            for(int i = 0; i < touchObjectPool.Count; i++)
            {
                // 만들어져 있는 것중에 사용 다하고 꺼져있는거 다시 사용하기
                if (!touchObjectPool[i].activeSelf)
                {
                    touchObjectPool[i].SetActive(true);
                    touchObjectPool[i].transform.localPosition = localPoint;
                    return;
                }
            }
            // 처음이거나 사용가능한게 없을때 새로 만들어서 넣어줌
            var gameobject = Instantiate(effect, canvas.transform);
            gameobject.transform.localPosition = localPoint;
            touchObjectPool.Add(gameobject);
        }
        TouchTime += Time.deltaTime;

#if UNITY_ANDROID
        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 시작된 상태인지 확인 (터치 시작 시)
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 위치를 월드 좌표로 변환
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

                // Z축을 0으로 설정하여 2D 평면에서의 위치를 맞춤
                touchPosition.z = 0f;

                // 이펙트 생성
                Instantiate(effect, touchPosition, Quaternion.identity);
            }
        }
        */
#endif
    }
}
