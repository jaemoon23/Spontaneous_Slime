using UnityEngine;

public class touchEffect : MonoBehaviour
{
    public GameObject touchEffectPrefab;
    public float distanceFromCamera = 1f;

    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Input.mousePosition;
            touchPos.z = distanceFromCamera;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            Instantiate(touchEffectPrefab, worldPos, Camera.main.transform.rotation);

            //Vector3(323.976807,45.7141571,1.05563265e-06) 카메라/터치 로테이션값

            if (audioClip != null)
            audioSource.PlayOneShot(audioClip);
            

        }
    }
}
