using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    const float MIN = 0.0001f; // log10(0) = 무한대이므로 0이 아닌 최소값 설정


    private void Start()
    {
        // 레지스트리에서 설정 불러오기
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);   // 값을 찾지 못하면 기본값 1
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1f);       // 값을 찾지 못하면 기본값 1
        
        // 불러온 값을 오디오 믹서에 적용
        ApplyMusic(musicSlider.value);
        ApplySFX(sfxSlider.value);


        // UI 이벤트에 함수 연결
        // onValueChanged >> UI 컴포넌트 값이 변경될 때 발생하는 이벤트
        // AddListener >> 이벤트가 실행 될 때 호출할 메서드
        musicSlider.onValueChanged.AddListener(ApplyMusic);
        sfxSlider.onValueChanged.AddListener(ApplySFX);
    }

    public void ApplyMusic(float musicVolume)
    {
        SetVolume("MusicVol", musicVolume);             // 오디오 믹서에 볼륨 설정
        PlayerPrefs.SetFloat("MusicVol", musicVolume);  // 레지스트리에 설정 저장
    }

    public void ApplySFX(float sfxVolume)
    {
        SetVolume("SFXVol", sfxVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
    }

    // 볼륨을 0~1 사이의 값으로 받아서 dB로 변환하여 오디오 믹서에 설정
    void SetVolume(string name, float volume)
    {
        float value = Mathf.Clamp(volume, MIN, 1f); // 주어진 값을 MIN~1 사이로 제한
        float dB = Mathf.Log10(value) * 20f;        // dB 변환 공식 dB = 20 * log10(선형값)
        mixer.SetFloat(name, dB);                   // name에 해당하는 파라미터에 dB 값 설정
    }
}
