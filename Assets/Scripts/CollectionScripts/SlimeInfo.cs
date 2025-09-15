using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlimeInfo : MonoBehaviour
{
    [SerializeField] private Button infoCloseButton;
    private GameObject collectionPanel; // 슬라임 정보 패널
    public TextMeshProUGUI slimeNameText;
    public TextMeshProUGUI slimeDescriptionText;
    public TextMeshProUGUI slimeStoryText;
    public Image slimeImage;
    private CollectionManager collectionManager;
    private GameObject collectionManagerObject; // 컬렉션 매니저 오브젝트 참조

    private void Start()
    {
        infoCloseButton.onClick.AddListener(OnCloseButton);

        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();
    }

    public void OnCloseButton()
    {
        Destroy(gameObject);
        collectionManager.OpenSlimeCollection();
    }
}
