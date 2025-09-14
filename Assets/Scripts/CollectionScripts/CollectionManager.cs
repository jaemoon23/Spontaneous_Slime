using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button collectionButtonClose;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private GameObject collectionUI;
    [SerializeField] private GameObject[] slimeCollectionPanels;    // 도감 패널

    [SerializeField] private List<CollectionSlot> slots = new List<CollectionSlot>();

    private int slotIndex = 0;
    private int pageIndex = 0;

    void Start()
    {
        pageIndex = 0;

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

        if (pageIndex > 0)
        {
            int temp = pageIndex;
            pageIndex--;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[pageIndex].SetActive(true);
        }
    }

    // 오른쪽 화살표 클릭 (다음 페이지)
    public void OnClickRightArrow()
    {
        if (pageIndex < slimeCollectionPanels.Length - 1)
        {
            int temp = pageIndex;
            pageIndex++;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[pageIndex].SetActive(true);
        }
    }

    public void AddCollection(string slimeId)
    {
        if (slotIndex >= slots.Count)
        {
            Debug.LogWarning("더 이상 도감 슬롯이 없습니다.");
            return;
        }
        slots[slotIndex].SetSlime(slimeId);
        slotIndex++;
    }
   
}