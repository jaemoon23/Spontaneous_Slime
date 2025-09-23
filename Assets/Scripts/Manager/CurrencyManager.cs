using System;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    
    public static CurrencyManager Instance { get; private set; }
    private const int MaxEther = 9999;
    public int Ether { get; private set; } = 0;

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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddEther(100); // C 키를 누르면 100 에테르 추가
        }
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
