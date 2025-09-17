using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public LayerMask touchLayers;
    [SerializeField] private GameObject slimeCollection;
    [SerializeField] private CollectionManager collectionManager;
    [SerializeField] private GameObject maxLevelPanel;
    [SerializeField] private GameObject choicePanel;
    private void Update()
    {
        // // 터치 입력 감지 (한 번만)
        // if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시작 시에만
        // {
        //     // 레이캐스트로 슬라임 터치 확인
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit hit))
        //     {
        //         SlimeGrowth slimeGrowth = hit.collider.GetComponent<SlimeGrowth>();
        //         if (slimeGrowth != null && !slimeCollection.activeSelf &&
        //             !collectionManager.IsInfoOpen && !maxLevelPanel.activeSelf &&
        //             !choicePanel.activeSelf)
        //         {
        //             slimeGrowth.OnTouch(); // 한 번만 호출
        //         }
        //     }
        // }
        
        // // 터치 입력 감지 (모바일)
        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Began) // 터치 시작 시에만
        //     {
        //         // 레이캐스트로 슬라임 터치 확인
        //         Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //         if (Physics.Raycast(ray, out RaycastHit hit))
        //         {
        //             SlimeGrowth slimeGrowth = hit.collider.GetComponent<SlimeGrowth>();
        //             if (slimeGrowth != null && !slimeCollection.activeSelf &&
        //             !collectionManager.IsInfoOpen && !maxLevelPanel.activeSelf &&
        //             !choicePanel.activeSelf)
        //             {
        //                 slimeGrowth.OnTouch(); // 한 번만 호출
        //             }
        //         }
        //     }
        // }

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
                !choicePanel.activeSelf)
                {
                    touchable.OnTouch();
                }
            }
        }

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
                !choicePanel.activeSelf)
                {
                    touchable.OnTouch();
                }
            }
        }


    }
}
