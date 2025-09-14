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

    GameObject slimeInfo;


    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();
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
        collectionPanel = GameObject.FindWithTag(Tags.CollectionPanel);
        collectionPanel.SetActive(false);
        slimeInfo = collectionManager.SlimeInfo();
        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        //var descriptionData = DataTableManager.StringTable.Get(slimeData.SlimeDescription);
        var StoryData = DataTableManager.StringTable.Get(slimeData.SlimeStory);

        slimeInfo.GetComponent<SlimeInfo>().slimeNameText.text = slimeNameText.text;
        //infoPanel.GetComponent<SlimeInfo>().slimeDescriptionText.text = slimeData.Description;
        slimeInfo.GetComponent<SlimeInfo>().slimeDescriptionText.text = "추가 할지 말지 정하는중";
        slimeInfo.GetComponent<SlimeInfo>().slimeStoryText.text = StoryData.Value;
        slimeInfo.GetComponent<SlimeInfo>().slimeImage.sprite = slimeIcon.sprite;
    }
    
}
    
    

