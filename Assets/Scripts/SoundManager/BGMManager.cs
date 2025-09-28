using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip rainClip;
    [SerializeField] private AudioSource rainSource;
    [SerializeField, Range(0f, 1f)] private float rainVolume = 0.5f;


    void Awake()
    {
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = volume;
        audioSource.Play();

        rainSource.clip = rainClip;
        rainSource.loop = true;
        rainSource.playOnAwake = false;
        rainSource.volume = rainVolume;

    }

    public void PlayBGM()
    {
        rainSource.Play();
    }
}
