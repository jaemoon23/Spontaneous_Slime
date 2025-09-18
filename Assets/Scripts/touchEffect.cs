using UnityEngine;

public class touchEffect : MonoBehaviour
{
    public GameObject touchEffectPrefab;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 모바일 터치 or PC 클릭
        {
            Vector3 touchPos = Input.mousePosition;
            touchPos.z = 10f; // 카메라에서 떨어진 거리
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            Instantiate(touchEffectPrefab, worldPos, Quaternion.identity);
        }
    }
}
