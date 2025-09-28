using UnityEngine;

public class isRain : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip isRain;
    [SerializeField, range(0f, 1f)] private float volume = 0.5f;
    void Awake()
    {
        bgmSource.clip = isRain;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.voulme = volume;

        // 비 이벤트 연결 시 호출되게
    }
}
