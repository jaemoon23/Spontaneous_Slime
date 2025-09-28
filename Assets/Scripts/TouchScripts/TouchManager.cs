using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public LayerMask touchLayers;
    [SerializeField] private GameObject slimeCollection;
    [SerializeField] private CollectionManager collectionManager;
    [SerializeField] private GameObject maxLevelPanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject mailPanel;
    [SerializeField] private MailManager mailManager;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject itemBuy;
    [SerializeField] private GameObject FurnitureItemUsePanel;
    [SerializeField] private GameObject ConsumableItemUsePanel;
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject tutorialPanel;
    private void Update()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        // 터치 입력이 있는지 확인 및 터치 시작 단계인지 확인
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // 터치 위치에서 레이캐스트 생성
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            // 레이캐스트가 터치 가능한 레이어에 닿았는지 확인
            // 레이어로 설정된 오브젝트만 터치 가능
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchLayers))
            {
                ITouchable touchable = hit.collider.GetComponent<ITouchable>();
                if (touchable != null && !slimeCollection.activeSelf &&
                !collectionManager.IsInfoOpen && !maxLevelPanel.activeSelf &&
                !choicePanel.activeSelf && !mailPanel.activeSelf && !mailManager.isMailOpen
                && !shopPanel.activeSelf && !itemBuy.activeSelf && !FurnitureItemUsePanel.activeSelf
                && !ConsumableItemUsePanel.activeSelf && !inventoryPanel.activeSelf && !tutorialPanel.activeSelf)
                {
                    touchable.OnTouch();
                }
            }
        }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 위치에서 레이캐스트 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 레이캐스트가 터치 가능한 레이어에 닿았는지 확인
            // 레이어로 설정된 오브젝트만 터치 가능
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchLayers))
            {
                ITouchable touchable = hit.collider.GetComponent<ITouchable>();
                if (touchable != null && !slimeCollection.activeSelf &&
                !collectionManager.IsInfoOpen && !maxLevelPanel.activeSelf &&
                !choicePanel.activeSelf && !mailPanel.activeSelf && !mailManager.isMailOpen
                && !shopPanel.activeSelf && !itemBuy.activeSelf && !FurnitureItemUsePanel.activeSelf
                && !ConsumableItemUsePanel.activeSelf && !inventoryPanel.activeSelf && !tutorialPanel.activeSelf)
                {
                    touchable.OnTouch();
                }
            }
        }
#endif
    }
}
