using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureItemUsePanel : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button closeButton;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [Header("GameObject")]
    [SerializeField] private GameObject windowObject;
    [SerializeField] private GameObject clockObject;
    [SerializeField] private GameObject woolenYarnObject;



    private InteriorData currentInteriorData;  // 현재 선택된 아이템 데이터
    private void Start()
    {
        useButton.onClick.AddListener(OnUseButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnUseButtonClick()
    {
        // 사용 버튼 클릭 시 처리할 로직
        var interiorName = DataTableManager.StringTable.Get(currentInteriorData.InteriorName).Value;
        switch (interiorName)
        {
            case "창문":
                windowObject.SetActive(true);
                break;
            case "시계":
                clockObject.SetActive(true);
                break;
            case "털실":
                InteriorManager.Instance.SetWoolenYarnActive(true);
                break;
        }
    }

    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void SetInteriorUsePanel(InteriorData interiorData, int count)
    {
        currentInteriorData = interiorData;
        //itemNameText.text = interiorData.ItemName;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        var nameString = DataTableManager.StringTable.Get(interiorData.InteriorName);
        var descriptionString = DataTableManager.StringTable.Get(interiorData.Description);

        itemNameText.text = nameString.Value;
        itemDescriptionText.text = descriptionString.Value;
    }
}
