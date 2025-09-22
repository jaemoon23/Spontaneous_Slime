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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddGold(100); // 예시: C 키를 누르면 100 골드 추가
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

    public void UpdateGoldUI()
    {
        goldText.text = Strings.Gold + ": " + Gold.ToString();
    }
}
