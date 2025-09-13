using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject windowPanel;

    [SerializeField] private GameObject scriptWindowPrefab;
    [SerializeField] private TextMeshProUGUI scriptWindowText;
    [SerializeField] private Transform canvasTransform;

    // 슬라임 UI 요소들
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;

    private SlimeManager slime;
    private GameObject slimeManager; // 슬라임 매니저 오브젝트 참조

    void Start()
    {
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();

        scriptWindowPrefab = Resources.Load<GameObject>(Paths.ScriptWindow);
        exitButton.onClick.AddListener(() => OnExit());
        
        // 슬라임 이벤트 구독
        SlimeGrowth.OnExpChanged += UpdateExpUI;
        SlimeGrowth.OnLevelChanged += UpdateLevelUI;
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
            levelText.text = $"레벨: {level}";
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
    }

    public void ShowScriptWindow()
    {
        // 스크립트 윈도우 생성
        GameObject window = Instantiate(scriptWindowPrefab, canvasTransform);
        window.transform.localPosition = new Vector3(0, 0, 0);
        TextMeshProUGUI windowText = window.GetComponentInChildren<TextMeshProUGUI>();
        windowText.text = slime.stringScripts;

        // 텍스트 크기에 맞춰서 윈도우 사이즈 조절
        RectTransform windowRect = window.GetComponent<RectTransform>();
        float padding = 40f;
        float targetWidth = windowText.preferredWidth + padding;
        windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        // 윈도우 애니메이션
        StartCoroutine(MoveWindowUpAndFade(window));
    }

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

}
