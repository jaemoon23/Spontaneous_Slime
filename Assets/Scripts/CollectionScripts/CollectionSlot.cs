using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionSlot : MonoBehaviour
{
    [SerializeField] private Image slimeIcon;
    [SerializeField] private TextMeshProUGUI slimeNameText;
    [SerializeField] private Image lockedOverlay; // 잠금 상태 오버레이

    private void Start()
    {
        SetSlime("11011"); // 초기 슬라임 설정 (예: ID 11011)
    }
    
    public void SetSlime(string slimeId)
    {
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIcon);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeName);

        Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
        slimeIcon.sprite = iconSprite;
        slimeNameText.text = nameData.Value;
    }
}
    
    

