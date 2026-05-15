using System.Collections;
using UnityEngine;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI warningText;

    [Header("Permanent Upgrade Costs")]
    public int damageUpgradeCost = 100;
    public int distanceUpgradeCost = 100;
    public int speedUpgradeCost = 120;
    public int inventoryUpgradeCost = 150;

    [Header("Permanent Upgrade Increments")]
    public float damageUpgradeAmount = 10f;
    public float distanceUpgradeAmount = 1f;
    public float speedUpgradeAmount = 0.5f;
    public int inventoryUpgradeAmount = 1;

    [Header("Buff Costs")]
    public int damageBuffCost = 80;
    public int distanceBuffCost = 80;
    public int speedBuffCost = 80;
    public int inventoryBuffCost = 80;

    [Header("Buff Increments")]
    public float damageBuffMultiplier = 0.1f;
    public float distanceBuffMultiplier = 0.1f;
    public float speedBuffAmount = 0.5f;
    public float inventoryBuffMultiplier = 0.1f;

    private Coroutine _warningRoutine;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (moneyText != null && PersistentManager.Instance != null)
        {
            moneyText.text = " " + PersistentManager.Instance.money + "$";
        }
    }

    public void BuyDamageUpgrade()
    {
        if (!TrySpend(damageUpgradeCost)) return;
        if (PersistentManager.Instance != null) PersistentManager.Instance.baseHookDamage += damageUpgradeAmount;
        UpdateUI();
        Debug.unityLogger.Log("Buying Damage Upgrade");
    }

    public void BuyDistanceUpgrade()
    {
        if (!TrySpend(distanceUpgradeCost)) return;
        if (PersistentManager.Instance != null) PersistentManager.Instance.baseHookDistance += distanceUpgradeAmount;
        UpdateUI();
    }

    public void BuySpeedUpgrade()
    {
        if (!TrySpend(speedUpgradeCost)) return;
        if (PersistentManager.Instance != null) PersistentManager.Instance.baseHookSpeed += speedUpgradeAmount;
        UpdateUI();
    }

    public void BuyInventoryUpgrade()
    {
        if (!TrySpend(inventoryUpgradeCost)) return;
        if (PersistentManager.Instance != null) PersistentManager.Instance.baseMaxInventory += inventoryUpgradeAmount;
        UpdateUI();
    }

    public void BuyDamageBuff()
    {
        if (!TrySpend(damageBuffCost)) return;
        if (DayManager.Instance != null) DayManager.Instance.damageMultiplier += damageBuffMultiplier;
        UpdateUI();
    }

    public void BuyDistanceBuff()
    {
        if (!TrySpend(distanceBuffCost)) return;
        if (DayManager.Instance != null) DayManager.Instance.distanceMultiplier += distanceBuffMultiplier;
        UpdateUI();
    }

    public void BuySpeedBuff()
    {
        if (!TrySpend(speedBuffCost)) return;
        if (DayManager.Instance != null) DayManager.Instance.speedBuff += speedBuffAmount;
        UpdateUI();
    }

    public void BuyInventoryBuff()
    {
        if (!TrySpend(inventoryBuffCost)) return;
        if (DayManager.Instance != null) DayManager.Instance.inventoryMultiplier += inventoryBuffMultiplier;
        UpdateUI();
    }

    private bool TrySpend(int cost)
    {
        if (PersistentManager.Instance == null) return false;

        if (PersistentManager.Instance.SpendMoney(cost)) return true;

        ShowWarning();
        return false;
    }

    public void ShowWarning()
    {
        if (warningText == null) return;

        if (_warningRoutine != null) StopCoroutine(_warningRoutine);
        _warningRoutine = StartCoroutine(WarningRoutine());
    }

    private IEnumerator WarningRoutine()
    {
        warningText.gameObject.SetActive(true);
        warningText.text = "Yetersiz Bakiye!";
        yield return new WaitForSeconds(2f);
        warningText.gameObject.SetActive(false);
    }
}

