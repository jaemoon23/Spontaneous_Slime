using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject windowPanel;

    private GameObject scriptWindowPrefab;
    [SerializeField] private Transform canvasTransform;

    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private GameObject MaxLevelPanel;
    [SerializeField] private GameObject slimeSpawnTextWindowPrefab;

    private SlimeManager slimeManager;
    private GameObject slimeManagerObject; // 슬라임 매니저 오브젝트 참조
    [SerializeField] private Button collectionButton;

    private void Start()
    {
        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();

        scriptWindowPrefab = Resources.Load<GameObject>(Paths.ScriptWindow);
        slimeSpawnTextWindowPrefab = Resources.Load<GameObject>(Paths.SlimeSpawnTextWindow);
        exitButton.onClick.AddListener(() => OnExit());

        // 슬라임 이벤트 구독
        SlimeGrowth.OnExpChanged += UpdateExpUI;
        SlimeGrowth.OnLevelChanged += UpdateLevelUI;

        // 저장된 UI 상태 로드
        LoadUIStates();
    }

    public void DisableExpUI(bool isActive)
    {
        expSlider.gameObject.SetActive(isActive);
        levelText.gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        SlimeGrowth.OnExpChanged -= UpdateExpUI;
        SlimeGrowth.OnLevelChanged -= UpdateLevelUI;
    }

    // 경험치 UI 업데이트
    public void UpdateExpUI(int currentExp, int maxExp)
    {
        if (expSlider != null)
        {
            expSlider.maxValue = 1f;
            expSlider.value = (float)currentExp / maxExp;
        }

        if (expText != null)
        {
            expText.text = $"{currentExp} / {maxExp}";
        }
    }

    // 레벨 UI 업데이트
    public void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }

    private void OnExit()
    {
        windowPanel.SetActive(false);
        for (int i = 0; i < windowPanel.transform.childCount; i++)
        {
            GameObject child = windowPanel.transform.GetChild(i).gameObject; // 자식 오브젝트 가져오기
            if (child == buttonObject)
            {
                continue; // buttonObject 오브젝트는 건너뜀
            }
            child.SetActive(false);
        }
        SaveUIStates(); // UI 상태 변경 저장
    }

    // 슬라임 스크립트 윈도우를 표시하는 메서드
    public void ShowScriptWindow()
    {
        // 스크립트 윈도우 생성
        GameObject window = Instantiate(scriptWindowPrefab, canvasTransform);
        window.transform.localPosition = new Vector3(0, 0, 0);
        TextMeshProUGUI windowText = window.GetComponentInChildren<TextMeshProUGUI>();
        windowText.text = slimeManager.StringScripts[UnityEngine.Random.Range(0, slimeManager.StringScripts.Length)];

        // 텍스트 크기에 맞춰서 윈도우 사이즈 조절
        RectTransform windowRect = window.GetComponent<RectTransform>();
        float padding = 40f;
        float targetWidth = windowText.preferredWidth + padding;
        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        // 윈도우 애니메이션
        StartCoroutine(MoveWindowUpAndFade(window));
    }

    // 윈도우를 위로 이동시키고 페이드 아웃하는 코루틴
    private IEnumerator MoveWindowUpAndFade(GameObject window)
    {
        Vector3 startPos = window.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 200, 0);
        float duration = 1f;
        float time = 0f;

        // CanvasGroup 추가 또는 가져오기
        // CanvasGroup 컴포넌트를 사용하여 윈도우의 알파 값을 조절
        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = window.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;

        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            canvasGroup.alpha -= Time.deltaTime / duration;
            if (canvasGroup.alpha < 0f)
            {
                canvasGroup.alpha = 0f;
            }
            window.transform.localPosition = Vector3.Lerp(startPos, endPos, time);

            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(window);
    }

    // 슬라임 등장 텍스트를 표시하고 페이드 아웃하는 메서드
    public void ShowSlimeSpawnText(string slimeName)
    {
        collectionButton.interactable = false; // 도감 버튼 비활성화
        // 스크립트 윈도우 생성
        GameObject window = Instantiate(slimeSpawnTextWindowPrefab, canvasTransform);
        window.transform.localPosition = new Vector3(0, 500, 0);

        // 텍스트 설정
        TextMeshProUGUI windowText = window.GetComponentInChildren<TextMeshProUGUI>();
        windowText.text = slimeName + "등장!";
        windowText.color = new Color(windowText.color.r, windowText.color.g, windowText.color.b, 1f); // 완전 불투명

        // 텍스트 크기에 맞춰서 윈도우 사이즈 조절
        RectTransform windowRect = window.GetComponent<RectTransform>();
        float padding = 40f;
        float targetWidth = windowText.preferredWidth + padding;
        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        StartCoroutine(FadeOutSlimeSpawnText(window));
    }

    public void ShowWarningText(string SlimeWarningScript)
    {
        // 경고 문구 가져오기
        var warningMessage = DataTableManager.StringTable.Get(SlimeWarningScript);
        if (warningMessage == null)
        {
            Debug.LogWarning("경고 메시지 데이터를 찾을 수 없습니다.");
            return;
        }

        // 스크립트 윈도우 생성
        GameObject window = Instantiate(scriptWindowPrefab, canvasTransform);
        window.transform.localPosition = new Vector3(0, 0, 0);
        TextMeshProUGUI windowText = window.GetComponentInChildren<TextMeshProUGUI>();
        windowText.text = warningMessage.Value;

        // 텍스트 크기에 맞춰서 윈도우 사이즈 조절
        RectTransform windowRect = window.GetComponent<RectTransform>();
        float padding = 40f;
        float targetWidth = windowText.preferredWidth + padding;
        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        StartCoroutine(MoveWindowUpAndFade(window));
    }

    // 슬라임 등장 텍스트 페이드 아웃 코루틴
    private IEnumerator FadeOutSlimeSpawnText(GameObject window)
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);

        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = window.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;

        // 페이드 아웃 시간 (0.5초)
        float fadeTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        // 완전히 투명하게 만들고 윈도우 파괴
        canvasGroup.alpha = 0f;
        Destroy(window);
    }

    // 맥스 레벨 패널 표시
    public void ShowMaxLevelPanel()
    {
        if (MaxLevelPanel != null)
        {
            MaxLevelPanel.SetActive(true);
            levelText.text = "MAX LEVEL";
            SaveUIStates(); // 패널 상태 변경 저장
        }
    }

    // UI 상태를 SaveData에 저장
    public void SaveUIStates()
    {
        var saveData = SaveLoadManager.Data;

        // UI 패널 활성화 상태 저장
        saveData.IsMaxLevelPanelOpen = MaxLevelPanel != null && MaxLevelPanel.activeSelf;
        saveData.IsChoiceUIActive = windowPanel != null && windowPanel.activeSelf;

        Debug.Log("UI 상태 저장됨");
    }

    // SaveData에서 UI 상태 로드
    public void LoadUIStates()
    {
        var saveData = SaveLoadManager.Data;

        // UI 패널 상태 복원
        if (MaxLevelPanel != null)
        {
            MaxLevelPanel.SetActive(saveData.IsMaxLevelPanelOpen);
        }

        if (windowPanel != null)
        {
            windowPanel.SetActive(saveData.IsChoiceUIActive);
        }

        Debug.Log("UI 상태 로드됨");
    }
}
