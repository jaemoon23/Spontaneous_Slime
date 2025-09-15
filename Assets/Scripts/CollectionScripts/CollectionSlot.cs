using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CollectionSlot : MonoBehaviour
{   
    CollectionManager collectionManager;
    private GameObject collectionManagerObject; // 컬렉션 매니저 오브젝트 참조
    public string SlimeId { get; private set; }
    [SerializeField] private Image slimeIcon;
    [SerializeField] private TextMeshProUGUI slimeNameText;
    private GameObject collectionPanel; // 슬라임 도감 패널

    private GameObject uiManagerObject; // UI 매니저 오브젝트 참조
    private UiManager uiManager;

    private GameObject slimeInfo; // 슬라임 정보 프리팹
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
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
        var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIcon);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeName);


        Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
        slimeIcon.sprite = iconSprite;
        slimeNameText.text = nameData.Value;
    }

    public void SetSlimeInfo()
    {
        
        //slimeInfo = collectionManager.SlimeInfo();
        if (SlimeId == null)
        {
            return;
        }
        collectionPanel = GameObject.FindWithTag(Tags.CollectionPanel);
        collectionPanel.SetActive(false);
        var slimeInfoGo = Instantiate(slimeInfo, uiManager.transform);
        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        //var descriptionData = DataTableManager.StringTable.Get(slimeData.SlimeDescription);
        var StoryData = DataTableManager.StringTable.Get(slimeData.SlimeStory);

        slimeInfoGo.GetComponent<SlimeInfo>().slimeNameText.text = slimeNameText.text;
        //infoPanel.GetComponent<SlimeInfo>().slimeDescriptionText.text = slimeData.Description;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeDescriptionText.text = "추가 할지 말지 정하는중";
        slimeInfoGo.GetComponent<SlimeInfo>().slimeStoryText.text = StoryData.Value;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeImage.sprite = slimeIcon.sprite;
    }
    
}
    
    

