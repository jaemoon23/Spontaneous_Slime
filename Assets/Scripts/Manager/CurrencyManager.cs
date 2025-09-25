using System;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    
    public static CurrencyManager Instance { get; private set; }
    private const int MaxEther = 9999;
    public int Ether { get; private set; } = 0;
    
    // 에테르 값을 설정하는 메서드 (세이브/로드용)
    public void SetEther(int amount)
    {
        Ether = Mathf.Max(0, amount);
        UpdateEtherUI();
        Debug.Log($"에테르 값 설정: {Ether}");
    }

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
        UpdateEtherUI();
    }

    public void AddEther(int amount)
    {
        if (Ether >= MaxEther)
        {
            Ether = MaxEther;
            UpdateEtherUI();
            return;
        }
        Ether += amount;
        UpdateEtherUI();
    }

    public bool RemoveGold(int amount)
    {
        if (Ether >= amount)
        {
            Ether -= amount;
            UpdateEtherUI();
            return true; // 성공적으로 차감됨
        }
        return false; // 에테르가 부족해서 차감 실패
    }

    public bool CanAfford(int amount)
    {
        return Ether >= amount;
    }

    public void UpdateEtherUI()
    {
        goldText.text = Ether.ToString();
    }
}
