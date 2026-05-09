using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static int totalFishValue = 0;

    public static void AddFish(int value)
    {
        totalFishValue += value;
        Debug.Log("Balık yakalandı! Kazanılan Değer: " + value + " | Toplam Bakiye: " + totalFishValue);
    }
}
