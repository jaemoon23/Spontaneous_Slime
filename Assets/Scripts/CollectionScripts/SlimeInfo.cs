using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlimeInfo : MonoBehaviour
{
    [SerializeField] private Button infoCloseButton;
    [SerializeField] private GameObject collectionPanel; // 슬라임 정보 패널
    public TextMeshProUGUI slimeNameText;
    public TextMeshProUGUI slimeDescriptionText;
    public TextMeshProUGUI slimeStoryText;
    public Image slimeImage;

    private void Start()
    {
        infoCloseButton.onClick.AddListener(OnCloseButton);
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
        collectionPanel.SetActive(true);
    }
}
