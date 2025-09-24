using UnityEngine;

public class InteriorManager : MonoBehaviour
{
    public static InteriorManager Instance { get; private set; }    // 싱글톤 인스턴스
    [SerializeField] private GameObject aircon; // 에어컨
    [SerializeField] private GameObject stove;  // 난로
    [SerializeField] private GameObject interiorLight; // 인테리어 조명
    [SerializeField] private GameObject flowerPot; // 화분
    [SerializeField] private GameObject humidifier; // 가습기
    [SerializeField] private GameObject window; // 창문
    [SerializeField] private GameObject clock; // 시계
    [SerializeField] private GameObject woolenYarn; // 양털

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
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }
    public bool GetAirconActive()
    {
        return aircon.activeSelf;
    }
    public bool GetStoveActive()
    {
        return stove.activeSelf;
    }
    public bool GetInteriorLightActive()
    {
        return interiorLight.activeSelf;
    }
    public bool GetFlowerPotActive()
    {
        return flowerPot.activeSelf;
    }
    public bool GetHumidifierActive()
    {
        return humidifier.activeSelf;
    }
    public bool GetWindowActive()
    {
        return window.activeSelf;
    }
    public bool GetClockActive()
    {
        return clock.activeSelf;
    }
    public bool GetWoolenYarnActive()
    {
        return woolenYarn.activeSelf;
    }

    public void SetWoolenYarnActive(bool isWoolenYarn)
    {
        woolenYarn.SetActive(isWoolenYarn);
    }

}
