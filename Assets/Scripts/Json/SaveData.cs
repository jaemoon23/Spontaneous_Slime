using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUpgrade();
}

[Serializable]
public class SaveDataV1 : SaveData
{
    public string SlimeName { get; set; }

    // 게임 진행 상태
    public bool IsOneCoin { get; set; } = false;
    public bool IsFirstStart { get; set; } = true;
    
    // 슬라임 상태
    public int CurrentSlimeId { get; set; } = 0;
    public int CurrentSlimeType { get; set; } = 0; // SlimeType enum을 int로 저장
    public bool SlimeDestroyed { get; set; } = false;
    public bool IsSlimeFree { get; set; } = false;

    // 슬라임 성장 데이터
    public int SlimeLevel { get; set; } = 1;
    public int SlimeCurrentExp { get; set; } = 0;
    public int SlimeLevelIndex { get; set; } = 0; // 레벨업 테이블 인덱스
    public int SlimeScaleLevel { get; set; } = 1;
    public bool SlimeIsMaxLevel { get; set; } = false;
    public bool SlimeIsStartCoroutine { get; set; } = false;
    
    // 환경 상태
    public int AirconTemp { get; set; } = 10;
    public int StoveStep { get; set; } = 0;
    public int LightStep { get; set; } = 0;
    public bool IsFlower { get; set; } = false;
    public int Humidity { get; set; } = 50;
    
    // 컬렉션 데이터
    public List<int> CollectedSlimeIds { get; set; } = new List<int>();
    public List<bool> SlimeDiscovered { get; set; } = new List<bool>();
    public int CollectionSlotIndex { get; set; } = 0; // 다음에 사용될 슬롯 인덱스
    public int CollectionPageIndex { get; set; } = 0; // 현재 페이지 인덱스
    
    // UI 상태 데이터
    public bool IsCollectionUIOpen { get; set; } = false;
    public bool IsInfoPanelOpen { get; set; } = false;
    public bool IsMaxLevelPanelOpen { get; set; } = false;
    public bool IsChoiceUIActive { get; set; } = false;
    
    // 게임 진행 데이터
    public float GameTime { get; set; } = 0f; // 총 게임 플레이 시간
    public int SlimeInteractionCount { get; set; } = 0; // 슬라임 터치 횟수
    public int SlimeGenerationCount { get; set; } = 0; // 슬라임 생성 횟수
    
    
    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUpgrade()
    {
        // 업그레이드 하면 여기에 구현
        return this;
    }
}
