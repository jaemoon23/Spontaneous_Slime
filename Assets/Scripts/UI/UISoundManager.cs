using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    public void PlayOpenSound()
    {
        if (audioSource != null && openClip != null)
            audioSource.PlayOneShot(openClip);
    }

    public void PlayCloseSound()
    {
        if (audioSource != null && closeClip != null)
            audioSource.PlayOneShot(closeClip);
    }
}
