using UnityEngine;

public class touchEffect : MonoBehaviour
{
    [SerializeField] private GameObject touchEffectPrefab;
    [SerializeField] private float distanceFromCamera = 1f;

    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
        Vector3 touchPos = Vector3.zero;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = Input.mousePosition;
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPos = Input.GetTouch(0).position;
            
        }
#endif
        if (touchPos != Vector3.zero)
        {
            if (audioClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
            touchPos.z = distanceFromCamera;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            Quaternion rotation = Camera.main.transform.rotation;

            if (touchEffectPrefab != null)
            {
                Instantiate(touchEffectPrefab, worldPos, rotation);
                
            }

        }
            

        // {
        //     if (Input.GetMouseButtonDown(0))
        //     Debug.Log("마우스 클릭 감지");
        //     {
        //         Debug.Log("터치 감지");

        //         Vector3 touchPos = Input.mousePosition;
        //         touchPos.z = distanceFromCamera;
        //         Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

        //         Instantiate(touchEffectPrefab, worldPos, Camera.main.transform.rotation);

        //         //Vector3(323.976807,45.7141571,1.05563265e-06) 카메라/터치 로테이션값
        //         Debug.Log("SpawnPos: " + worldPos);

        //         if (audioClip != null)
        //             audioSource.PlayOneShot(audioClip);


        //     }
        // }
        

    }
}
