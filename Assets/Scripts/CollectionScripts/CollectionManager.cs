using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;

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
    public List<SlimeData> slimeDatas = new List<SlimeData>();

    [SerializeField] private GameObject infoPanel; // 슬라임 정보 패널

    public bool IsInfoOpen { get; set; } = false;

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

    public void AddCollection(SlimeData slimeData)
    {
        if (slotIndex >= slots.Count)
        {
            Debug.LogWarning("더 이상 도감 슬롯이 없습니다.");
            return;
        }

        if (slots.Exists(slot => slot.SlimeId == slimeData.SlimeId))
        {
            Debug.LogWarning("이미 도감에 추가된 슬라임입니다.");
            return;
        }
        slots[slotIndex].SetSlime(slimeData);
        slotIndex++;
    }
    
    public void OpenSlimeCollection()
    {
        collectionUI.SetActive(true);
    }

}