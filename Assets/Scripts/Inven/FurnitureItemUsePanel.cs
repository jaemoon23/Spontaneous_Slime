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
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("GameObject")]
    [SerializeField] private GameObject windowObject;
    [SerializeField] private GameObject clockObject;
    [SerializeField] private GameObject woolenYarnObject;
    [SerializeField] private GameObject wallObject;
    [SerializeField] private GameObject invenPanel;



    private InteriorData currentInteriorData;  // 현재 선택된 아이템 데이터
    private void Start()
    {
        useButton.onClick.AddListener(OnUseButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
        warningText.text = string.Empty;
    }

    private void OnUseButtonClick()
    {
        // 사용 버튼 클릭 시 처리할 로직
        var interiorName = DataTableManager.StringTable.Get(currentInteriorData.InteriorName).Value;
        switch (interiorName)
        {
            case "창문":
                if (InteriorManager.Instance.GetWindowActive())
                {
                    warningText.text = "창문이 이미 설치되어 있습니다!";
                    return;
                }
                windowObject.SetActive(true);
                wallObject.SetActive(false);
                gameObject.SetActive(false);
                invenPanel.SetActive(false);
                break;
            case "시계":
                if (InteriorManager.Instance.GetClockActive())
                {
                    warningText.text = "시계가 이미 설치되어 있습니다!";
                    return;
                }
                clockObject.SetActive(true);
                invenPanel.SetActive(false);
                gameObject.SetActive(false);
                break;
            case "털실":
                if (InteriorManager.Instance.GetWoolenYarnActive())
                {
                    warningText.text = "털실이 이미 설치되어 있습니다!";
                    return;
                }
                InteriorManager.Instance.SetWoolenYarnActive(true);
                invenPanel.SetActive(false);
                gameObject.SetActive(false);
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

        itemDescriptionText.text = descriptionString.Value.Split("\\n")[0];
        warningText.text = string.Empty;
    }
}
  