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
    public int cumulativeExp { get; set; } = 0; // 누적 경험치
    // 벽 활성 여부
    public bool IsWallActive { get; set; } = true;
    // 게임 진행 상태
    public bool IsOneCoin { get; set; } = false;
    public bool IsFirstStart { get; set; } = true;
    
    // 슬라임 상태
    public int CurrentSlimeId { get; set; } = 0;
    public int CurrentSlimeType { get; set; } = 0; // SlimeType enum을 int로 저장
    public bool SlimeDestroyed { get; set; } = false;
    public bool IsSlimeFree { get; set; } = false;
    public bool IsFromSummonStone { get; set; } = false; // 소환석으로 생성된 슬라임인지 여부

    // 슬라임 성장 데이터
    public int SlimeLevel { get; set; } = 1;
    public int SlimeCurrentExp { get; set; } = 0;
    public int SlimeLevelIndex { get; set; } = 0; // 레벨업 테이블 인덱스
    public int SlimeScaleLevel { get; set; } = 1;
    public int SlimeMaxExp { get; set; } = 10;
    public bool SlimeIsMaxLevel { get; set; } = false;
    public bool SlimeIsStartCoroutine { get; set; } = false;
    public Vector3 SlimeScale { get; set; } = new Vector3(3, 3, 3);
    
    // 환경 상태
    public int AirconTemp { get; set; } = 10;
    public int StoveStep { get; set; } = 0;
    public int LightStep { get; set; } = 0;
    public bool IsFlower { get; set; } = false;
    public int Humidity { get; set; } = 50;
    
    // 환경 오브젝트 활성 상태
    public bool IsAirConditionerActive { get; set; } = false;
    public bool IsHumidifierActive { get; set; } = false;
    public bool IsLightActive { get; set; } = false;
    public bool IsStoveActive { get; set; } = false;
    public bool IsFlowerPotActive { get; set; } = false;
    public bool IsWindowActive { get; set; } = false; // 창문
    public bool IsClockActive { get; set; } = false; // 시계
    public bool IsWoolenYarnActive { get; set; } = false; // 양털
    
    // 컬렉션 데이터
    public List<int> CollectedSlimeIds { get; set; } = new List<int>();
    public List<bool> SlimeDiscovered { get; set; } = new List<bool>();
    public int CollectionSlotIndex { get; set; } = 0; // 다음에 사용될 슬롯 인덱스
    public int CollectionPageIndex { get; set; } = 0; // 현재 페이지 인덱스
    public int CollectionSortType { get; set; } = 0; // 도감 정렬 타입 (0: 획득순, 1: 희귀도순, 2: 이름순)
    public Dictionary<int, string> CollectionTimes { get; set; } = new Dictionary<int, string>();
    
    // UI 상태 데이터
    public bool IsCollectionUIOpen { get; set; } = false;
    public bool IsInfoPanelOpen { get; set; } = false;
    public bool IsMaxLevelPanelOpen { get; set; } = false;
    public bool IsChoiceUIActive { get; set; } = false;
    
    // 게임 진행 데이터
    public float GameTime { get; set; } = 0f; // 총 게임 플레이 시간
    public int SlimeInteractionCount { get; set; } = 0; // 슬라임 터치 횟수
    public int SlimeGenerationCount { get; set; } = 0; // 슬라임 생성 횟수
    
    // 화폐 데이터
    public int Ether { get; set; } = 0; // 에테르 (게임 내 화폐)
    
    // 시간/날씨 데이터
    public float CurrentTime { get; set; } = 0f; // 현재 시간 (0 ~ dayDuration)
    public int DayCount { get; set; } = 1; // 경과한 일수
    public int CurrentTimeOfDay { get; set; } = 0; // 현재 시간대 (TimeState enum)
    public int CurrentWeather { get; set; } = 0; // 현재 날씨 (WeatherState enum)
    public float DayDuration { get; set; } = 120f; // 하루의 총 길이
    
    // 게임 진행 플래그
    public bool IsCat { get; set; } = false; // 고양이 슬라임 출현 여부
    
    // 인벤토리 데이터
    public Dictionary<int, int> ConsumableItems { get; set; } = new Dictionary<int, int>(); // 소비성 아이템 (ItemId, Count)
    public Dictionary<int, int> FurnitureItems { get; set; } = new Dictionary<int, int>(); // 가구 아이템 (ItemId, Count)
    
    // 메일 시스템 데이터
    public HashSet<string> ReceivedMailIds { get; set; } = new HashSet<string>(); // 이미 받은 메일 ID 목록
    public HashSet<string> ReadMailIds { get; set; } = new HashSet<string>(); // 읽은 메일 ID 목록
    public Dictionary<string, string> MailContents { get; set; } = new Dictionary<string, string>(); // 메일별 고정 내용
    public Dictionary<int, int> SlimeLetterIndex { get; set; } = new Dictionary<int, int>(); // 슬라임별 현재 편지 인덱스 (0-2, 3이면 편지 완료)
    
    
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
