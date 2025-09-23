using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    private Button button;
    public GameObject Panel;
    private int count = 0;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Panel.SetActive(true);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
    public void SetItem(InteriorData interiorData, int count)
    {
        // 아이템 데이터를 슬롯에 설정하는 로직 구현
        
        this.count = count;
    }
    
}