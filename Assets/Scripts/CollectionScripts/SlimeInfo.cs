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

    [SerializeField] public TextMeshProUGUI countText;
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
        // InvenManager 참조 얻기
        var invenManagerObj = GameObject.FindWithTag(Tags.InvenManager);
        if (invenManagerObj == null)
        {
            Debug.LogError("InvenManager를 찾을 수 없습니다.");
            return;
        }
        var invenManager = invenManagerObj.GetComponent<InvenManager>();
        if (invenManager == null)
        {
            Debug.LogError("InvenManager 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 소환석 데이터 가져오기 (소환석 ID: 10105)
        var summonStoneData = DataTableManager.ItemTable.Get(10105);
        if (summonStoneData == null)
        {
            Debug.LogError("소환석 데이터를 찾을 수 없습니다.");
            return;
        }

        // 소환석 개수 확인 및 차감
        if (!invenManager.RemoveConsumableItem(summonStoneData, 1))
        {
            Debug.Log("소환석이 부족합니다.");
            return; // 소환석이 없으면 리턴
        }

        // 소환석이 있어서 소환 성공
        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        if (slimeData == null)
        {
            Debug.LogError($"슬라임 데이터가 없습니다. SlimeId: {slimeId}");
            return;
        }
        
        slimeManager.CreateSlime((SlimeType)slimeData.SlimeTypeId, true, true, true); // 소환석으로 생성됨을 표시
        // 소환석 개수 실시간으로 가져오기
        countText.text = $"보유 수량: {invenManager.GetConsumableItemCount(10105)}";
        
        Debug.Log("소환석을 사용하여 슬라임을 소환했습니다. 소환석 개수가 1개 차감되었습니다.");
    }
}
