using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Balık Özellikleri")]
    public float health = 100f;
    public float speed = 3f;
    public int fishValue = 15;
    public float sizeMultiplier = 1f;

    [Header("Görsel Sıralama")]
    public string sortingLayerName = "Fish";
    public int sortingOrder = 10;

    // Kameranın dışına çıkınca silinmesi için sınır değeri
    public float leftScreenBound = -25f;

    private bool isCaught = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Hasar yediğinde rengin kırmızı kalma süresi
    private float flashTimer = 0f;

    void Start()
    {
        transform.localScale = Vector3.one * sizeMultiplier;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortingOrder;
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // DÜZELTME: Invoke yerine Timer kullanılarak aşırı yüklenme (kasma) sorunu kökünden çözüldü.
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0 && spriteRenderer != null)
            {
                spriteRenderer.color = originalColor; // Süre bitince orijinal renge dön
            }
        }

        if (!isCaught)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (transform.position.x < leftScreenBound)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isCaught) return;

        health -= damage;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            flashTimer = 0.1f; // 0.1 saniye boyunca kırmızı kalsın
        }

        if (health <= 0)
        {
            isCaught = true;

            if (FishingManager.Instance != null)
            {
                FishingManager.Instance.AddMoney(fishValue);
            }

            Destroy(gameObject);
        }
    }
}