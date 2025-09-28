using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = volume;

        audioSource.Play();

    }
}
