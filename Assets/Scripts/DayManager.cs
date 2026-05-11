using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Temporary Buffs (Reset Each Day)")]
    public float damageMultiplier = 1f;
    public float distanceMultiplier = 1f;
    public float inventoryMultiplier = 1f;
    public float speedBuff = 0f;

    private void Awake()
    {
        Instance = this;
        ResetDay();
    }

    public void ResetDay()
    {
        damageMultiplier = 1f;
        distanceMultiplier = 1f;
        inventoryMultiplier = 1f;
        speedBuff = 0f;
    }

    public float GetFinalHookDamage()
    {
        if (PersistentManager.Instance == null) return 0f;
        return PersistentManager.Instance.baseHookDamage * damageMultiplier;
    }

    public float GetFinalHookDistance()
    {
        if (PersistentManager.Instance == null) return 0f;
        return PersistentManager.Instance.baseHookDistance * distanceMultiplier;
    }

    public float GetFinalHookSpeed()
    {
        if (PersistentManager.Instance == null) return 0f;
        return PersistentManager.Instance.baseHookSpeed + speedBuff;
    }

    public int GetFinalMaxInventory()
    {
        if (PersistentManager.Instance == null) return 0;
        return Mathf.Max(1, Mathf.RoundToInt(PersistentManager.Instance.baseMaxInventory * inventoryMultiplier));
    }
}

