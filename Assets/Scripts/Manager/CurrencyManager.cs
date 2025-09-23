using System;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    
    public static CurrencyManager Instance { get; private set; }
    private const int MaxGold = 9999;
    public int Gold { get; private set; } = 0;

    [SerializeField] TextMeshProUGUI goldText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        UpdateGoldUI();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddGold(100); // C 키를 누르면 100 골드 추가
        }
    }
    public void AddGold(int amount)
    {
        if (Gold >= MaxGold)
        {
            Gold = MaxGold;
            UpdateGoldUI();
            return;
        }
        Gold += amount;
        UpdateGoldUI();
    }

    public bool RemoveGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            UpdateGoldUI();
            return true; // 성공적으로 차감됨
        }
        return false; // 골드가 부족해서 차감 실패
    }

    public bool CanAfford(int amount)
    {
        return Gold >= amount;
    }

    public void UpdateGoldUI()
    {
        goldText.text = Gold.ToString();
    }
}
