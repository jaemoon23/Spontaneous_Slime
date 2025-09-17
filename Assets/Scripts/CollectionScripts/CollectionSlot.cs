using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CollectionSlot : MonoBehaviour
{   
    CollectionManager collectionManager;
    private GameObject collectionManagerObject; // 컬렉션 매니저 오브젝트 참조
    public int SlimeId { get; private set; }
    [SerializeField] private Image slimeIcon;
    [SerializeField] private TextMeshProUGUI slimeNameText;
    private GameObject collectionPanel; // 슬라임 도감 패널

    private GameObject uiManagerObject; // UI 매니저 오브젝트 참조
    private UiManager uiManager;

    private CollectionManager colManager;
    private GameObject colManagerObject; // 컬렉션 매니저 오브젝트 참조
    private GameObject slimeInfo; // 슬라임 정보 프리팹
    private Button button;

    public bool IsInfoOpen = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        colManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        colManager = colManagerObject.GetComponent<CollectionManager>();

        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();

        slimeInfo = Resources.Load<GameObject>(Paths.SlimeInfo);

        button.onClick.AddListener(SetSlimeInfo);
    }

    public void SetSlime(SlimeData slimeData)
    {
        SlimeId = slimeData.SlimeId;
        var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIconId);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);


        Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
        slimeIcon.sprite = iconSprite;
        slimeNameText.text = nameData.Value;
    }

    public void SetSlimeInfo()
    {
        if (SlimeId == 0)
        {
            return;
        }
        var slimeInfoGo = Instantiate(slimeInfo, uiManager.transform);
        collectionManager.IsInfoOpen = true;

        collectionPanel = GameObject.FindWithTag(Tags.CollectionPanel);
        collectionPanel.SetActive(false);

        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        var InfoData = DataTableManager.StringTable.Get(slimeData.SlimeInformationId);
        var StoryData = DataTableManager.StringTable.Get(slimeData.SlimeStoryId);

        slimeInfoGo.GetComponent<SlimeInfo>().slimeNameText.text = slimeNameText.text;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeDescriptionText.text = InfoData.Value;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeStoryText.text = StoryData.Value;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeImage.sprite = slimeIcon.sprite;
    }
    
}
    
    

