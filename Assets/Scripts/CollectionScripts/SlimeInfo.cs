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

    private GameObject slimeManagerObject; // 슬라임 매니저 오브젝트 참조
    private SlimeManager slimeManager;
    [SerializeField] private Button recallButton;

    public int slimeId;

    private CollectionManager collectionManager;
    private GameObject collectionManagerObject; // 컬렉션 매니저 오브젝트 참조



    private void Start()
    {
        infoCloseButton.onClick.AddListener(OnCloseButton);
        recallButton.onClick.AddListener(UseStone);
    
        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();
    }

    private void OnDestroy()
    {
        collectionManager.IsInfoOpen = false;
        collectionManager.OpenSlimeCollection();
    }

    public void OnCloseButton()
    {
        Destroy(gameObject);

    }

    private void UseStone()
    {
        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        if (slimeData == null)
        {
            Debug.LogError($"슬라임 데이터가 없습니다. SlimeId: {slimeId}");
            return;
        }
        slimeManager.CreateSlime((SlimeType)slimeData.SlimeTypeId, true, true, true); // 소환석으로 생성됨을 표시
    }
}
