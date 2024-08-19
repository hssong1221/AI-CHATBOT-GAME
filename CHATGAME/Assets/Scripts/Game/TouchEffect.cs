using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    public Canvas canvas;
    public GameObject effect;

    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Instantiate(effect, pos, Quaternion.identity, canvas.transform);
        }
#endif
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
