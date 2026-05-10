using UnityEngine;
using TMPro;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;

    [Header("Referanslar")]
    public Transform ropeOrigin;
    public Transform hookTransform;
    public HookController hookController;

    [Header("Arayüz (UI) Yazıları")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI speedButtonText;    // Hız butonu yazısı
    public TextMeshProUGUI damageButtonText;   // Hasar butonu yazısı
    public TextMeshProUGUI distanceButtonText; // Derinlik/Mesafe butonu yazısı[Header("Ekonomi ve Geliştirmeler")]
    public int money = 0;
    public int speedLevel = 1;
    public int damageLevel = 1;
    public int distanceLevel = 1;
    public int maxLevel = 10;

    [Header("Temel Değerler (1. Seviye)")]
    public float baseSpeed = 5f;
    public float baseDamage = 50f;
    public float baseDepth = -10f; [Header("Artış Miktarları (Her Seviyede)")]
    public float speedIncrease = 1.5f;
    public float damageIncrease = 25f;
    public float depthIncrease = -5f; [Header("Fiyat Ayarları")]
    public int baseUpgradeCost = 100;

    [Header("Input")]
    public bool enableManualToggle = false;

    private bool isFishingMode = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateAllUI(); // Oyun başında tüm yazıları güncelle
    }

    void Update()
    {
        if (enableManualToggle && Input.GetKeyDown(KeyCode.E))
        {
            ToggleFishingMode();
        }
    }

    void ToggleFishingMode()
    {
        isFishingMode = !isFishingMode;
        hookController.SetFishingMode(isFishingMode);
        if (!isFishingMode) hookTransform.position = ropeOrigin.position;
    }

    // --- UPGRADE SİSTEMİ MATEMATİĞİ ---
    public float GetCurrentSpeed() => baseSpeed + (speedLevel - 1) * speedIncrease;
    public float GetCurrentDamage() => baseDamage + (damageLevel - 1) * damageIncrease;
    public float GetCurrentMaxDepth() => baseDepth + (distanceLevel - 1) * depthIncrease;
    public int GetCost(int level) => baseUpgradeCost * level;

    // --- UI (ARAYÜZ) GÜNCELLEMELERİ ---
    public void UpdateAllUI()
    {
        if (moneyText != null)
            moneyText.text = "Para: " + money + "$";

        if (speedButtonText != null)
            speedButtonText.text = speedLevel >= maxLevel ? "Hız MAX" : $"Hız (Lv.{speedLevel})\nFiyat: {GetCost(speedLevel)}$";

        if (damageButtonText != null)
            damageButtonText.text = damageLevel >= maxLevel ? "Hasar MAX" : $"Hasar (Lv.{damageLevel})\nFiyat: {GetCost(damageLevel)}$";

        if (distanceButtonText != null)
            distanceButtonText.text = distanceLevel >= maxLevel ? "Derinlik MAX" : $"Derinlik (Lv.{distanceLevel})\nFiyat: {GetCost(distanceLevel)}$";
    }

    // --- SATIN ALMA FONKSİYONLARI ---
    public void BuySpeedUpgrade()
    {
        if (speedLevel >= maxLevel) return;
        int cost = GetCost(speedLevel);
        if (money >= cost) { money -= cost; speedLevel++; UpdateAllUI(); }
    }

    public void BuyDamageUpgrade()
    {
        if (damageLevel >= maxLevel) return;
        int cost = GetCost(damageLevel);
        if (money >= cost) { money -= cost; damageLevel++; UpdateAllUI(); }
    }

    public void BuyDistanceUpgrade()
    {
        if (distanceLevel >= maxLevel) return;
        int cost = GetCost(distanceLevel);
        if (money >= cost) { money -= cost; distanceLevel++; UpdateAllUI(); }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateAllUI();
    }
}