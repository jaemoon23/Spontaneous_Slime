using UnityEngine;
using UnityEngine.UI;

public class FurnitureItemUsePanel : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        useButton.onClick.AddListener(OnUseButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnUseButtonClick()
    {
        // 사용 버튼 클릭 시 처리할 로직
    }

    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }
}
