using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; } // 싱글톤 인스턴스
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트 파괴
        }
    }
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
