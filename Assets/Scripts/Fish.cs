using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Balık Özellikleri")]
    public float health = 100f;
    public float speed = 3f;
    public int fishValue = 15;
    public float sizeMultiplier = 1f;

    private bool isCaught = false;

    void Start()
    {
        // Inspector'dan girilen boyutu uygula
        transform.localScale = Vector3.one * sizeMultiplier;
    }

    void Update()
    {
        // Balık yakalanmadıysa sürekli sola doğru yüzsün (Jam taktiği: Basit Translate)
        if (!isCaught)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    // Bu fonksiyonu HookController çağıracak
    public void TakeDamage(float damage)
    {
        if (isCaught) return; // Zaten yakalandıysa hasar alma

        health -= damage;

        // Görsel geri bildirim için balığı anlık kızartabilir veya titretebilirsin (Jam'de çok iş yapar!)

        if (health <= 0)
        {
            isCaught = true;
            InventoryManager.AddFish(fishValue);
            Destroy(gameObject);
        }
    }
}
