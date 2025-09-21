// CollectionManager에서 물음표 스프라이트 사용 예시

// 전체 슬라임 목록을 표시하는 방법 (수집된 것과 미수집된 것 모두)
private void UpdateSlotsWithUnknown()
{
    // 전체 슬라임 데이터 가져오기
    var allSlimes = DataTableManager.SlimeTable.GetAll();
    var collectedIds = SaveLoadManager.Data.CollectedSlimeIds;
    
    for (int i = 0; i < slots.Count && i < allSlimes.Count; i++)
    {
        var slimeData = allSlimes[i];
        
        if (collectedIds.Contains(slimeData.SlimeId))
        {
            // 수집된 슬라임 - 실제 이미지와 이름 표시
            slots[i].SetSlime(slimeData);
        }
        else
        {
            // 수집되지 않은 슬라임 - 물음표 표시
            slots[i].SetUnknownSlime(slimeData);
        }
    }
    
    // 남은 슬롯들은 빈 상태로
    for (int i = allSlimes.Count; i < slots.Count; i++)
    {
        slots[i].SetEmptySlot();
    }
}

// 또는 현재 방식 유지 (수집된 것만 표시)
private void UpdateSlotsCollectedOnly()
{
    var collectedSlimes = new List<SlimeData>();
    
    foreach (var slimeId in SaveLoadManager.Data.CollectedSlimeIds)
    {
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        if (slimeData != null)
        {
            collectedSlimes.Add(slimeData);
        }
    }
    
    // 정렬 적용 후...
    
    for (int i = 0; i < slots.Count; i++)
    {
        if (i < collectedSlimes.Count)
        {
            slots[i].SetSlime(collectedSlimes[i]);
        }
        else
        {
            slots[i].SetEmptySlot(); // 아무것도 표시하지 않음
        }
    }
}