using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button collectionButtonClose;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private GameObject collectionUI;
    [SerializeField] private GameObject[] slimeCollectionPanels;    // 도감 패널
    
    
    private int index = 0;
    
    void Start()
    {
        index = 0;

        // 도감 활성 비활성
        collectionButton.onClick.AddListener(OpenCollectionUI);
        collectionButtonClose.onClick.AddListener(CloseCollectionUI);

        // 페이지 넘기기
        leftArrowButton.onClick.AddListener(OnClickLeftArrow);
        rightArrowButton.onClick.AddListener(OnClickRightArrow);
    }


    // 도감 UI 열기
    public void OpenCollectionUI()
    {
        collectionUI.SetActive(true);
        collectionButton.gameObject.SetActive(false);
    }


    // 도감 UI 닫기
    public void CloseCollectionUI()
    {
        collectionUI.SetActive(false);
        collectionButton.gameObject.SetActive(true);
    }

    // 왼쪽 화살표 클릭 (이전 페이지)
    public void OnClickLeftArrow()
    {
        // 기존 패널 방식 사용
        if (index > 0)
        {
            int temp = index;
            index--;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[index].SetActive(true);
        }
    }

    // 오른쪽 화살표 클릭 (다음 페이지)
    public void OnClickRightArrow()
    {
        // 기존 패널 방식 사용
        if (index < slimeCollectionPanels.Length - 1)
        {
            int temp = index;
            index++;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[index].SetActive(true);
        }
    }

   
}