using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance;

    [Header("Core")]
    public int money = 0;
    public int fishCount = 0;
    public int currentDay = 1;

    [Header("Base Values (Permanent)")]
    public int baseMaxInventory = 10;
    public float baseHookDistance = 10f;
    public float baseHookDamage = 50f;
    public float baseHookSpeed = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int amount)
    {
        money += Mathf.Max(0, amount);
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        money -= amount;
        return true;
    }

    public void AddFish(int amount)
    {
        fishCount += Mathf.Max(0, amount);
    }

    public void ResetFish()
    {
        fishCount = 0;
    }
}

