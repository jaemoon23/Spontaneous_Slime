using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CollectionSlot : MonoBehaviour
{
    public string SlimeId { get; private set; }
    [SerializeField] private Image slimeIcon;
    [SerializeField] private TextMeshProUGUI slimeNameText;
    [SerializeField] private Image lockedOverlay; // 잠금 상태 오버레이

    public void SetSlime(SlimeData slimeData)
    {
        SlimeId = slimeData.SlimeId;
        var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIcon);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeName);

        
        Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
        slimeIcon.sprite = iconSprite;
        slimeNameText.text = nameData.Value;
    }
}
    
    

