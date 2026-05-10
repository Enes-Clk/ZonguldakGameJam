using UnityEngine;
using TMPro;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;

    [Header("Referanslar")]
    public Transform ropeOrigin;
    public Transform hookTransform;
    public HookController hookController;

    [Header("Kameralar (Geçiş İçin)")]
    public GameObject vcamShip; // Gemi kamerası
    public GameObject vcamHook; // Kanca kamerası[Header("Arayüz (UI) Yazıları")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI speedButtonText;
    public TextMeshProUGUI damageButtonText;
    public TextMeshProUGUI distanceButtonText;

    [Header("Ekonomi ve Geliştirmeler")]
    public int money = 0;
    public int speedLevel = 1;
    public int damageLevel = 1;
    public int distanceLevel = 1;
    public int maxLevel = 10;

    [Header("Temel Değerler (1. Seviye)")]
    public float baseSpeed = 5f;
    public float baseDamage = 50f;
    public float baseDepth = -10f;

    [Header("Artış Miktarları (Her Seviyede)")]
    public float speedIncrease = 1.5f;
    public float damageIncrease = 25f;
    public float depthIncrease = -5f;

    [Header("Fiyat Ayarları")]
    public int baseUpgradeCost = 100;

    public bool isFishingMode = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateAllUI();

        // Oyun başında kanca kamerasını kapat, gemi kamerasını aç ki oyun gemide başlasın
        if (vcamShip != null) vcamShip.SetActive(true);
        if (vcamHook != null) vcamHook.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleFishingMode();
        }
    }

    void ToggleFishingMode()
    {
        isFishingMode = !isFishingMode;

        if (hookController != null)
        {
            hookController.SetFishingMode(isFishingMode);
        }

        if (!isFishingMode)
        {
            // BALIK TUTMA İPTAL EDİLDİĞİNDE (E'ye basınca)
            if (hookTransform != null && ropeOrigin != null)
            {
                hookTransform.position = ropeOrigin.position;
            }

            // Gemi kamerasına geri dön
            if (vcamShip != null) vcamShip.SetActive(true);
            if (vcamHook != null) vcamHook.SetActive(false);
        }
        else
        {
            // BALIK TUTMAYA BAŞLANDIĞINDA
            // Kanca kamerasına geç
            if (vcamShip != null) vcamShip.SetActive(false);
            if (vcamHook != null) vcamHook.SetActive(true);
        }
    }

    public float GetCurrentSpeed() => baseSpeed + (speedLevel - 1) * speedIncrease;
    public float GetCurrentDamage() => baseDamage + (damageLevel - 1) * damageIncrease;
    public float GetCurrentMaxDepth() => baseDepth + (distanceLevel - 1) * depthIncrease;
    public int GetCost(int level) => baseUpgradeCost * level;

    public void UpdateAllUI()
    {
        if (moneyText != null) moneyText.text = "Para: " + money + "$";
        if (speedButtonText != null) speedButtonText.text = speedLevel >= maxLevel ? "Hız MAX" : $"Hız (Lv.{speedLevel})\nFiyat: {GetCost(speedLevel)}$";
        if (damageButtonText != null) damageButtonText.text = damageLevel >= maxLevel ? "Hasar MAX" : $"Hasar (Lv.{damageLevel})\nFiyat: {GetCost(damageLevel)}$";
        if (distanceButtonText != null) distanceButtonText.text = distanceLevel >= maxLevel ? "Derinlik MAX" : $"Derinlik (Lv.{distanceLevel})\nFiyat: {GetCost(distanceLevel)}$";
    }

    public void BuySpeedUpgrade() { if (speedLevel >= maxLevel) return; int cost = GetCost(speedLevel); if (money >= cost) { money -= cost; speedLevel++; UpdateAllUI(); } }
    public void BuyDamageUpgrade() { if (damageLevel >= maxLevel) return; int cost = GetCost(damageLevel); if (money >= cost) { money -= cost; damageLevel++; UpdateAllUI(); } }
    public void BuyDistanceUpgrade() { if (distanceLevel >= maxLevel) return; int cost = GetCost(distanceLevel); if (money >= cost) { money -= cost; distanceLevel++; UpdateAllUI(); } }
    public void AddMoney(int amount) { money += amount; UpdateAllUI(); }
}