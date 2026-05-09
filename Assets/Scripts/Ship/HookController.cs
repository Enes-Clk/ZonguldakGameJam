using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("Kanca Kontrol Ayarları")]
    public float moveSpeed = 5f;
    public float damagePerSecond = 50f;

    [Header("Başlangıç Fırlatma Gücü (AddForce Simülasyonu)")]
    public float initialThrowForce = 15f; // Suya ilk girdiğinde aşağı çeken ani güç
    public float forceDecaySpeed = 10f;   // Bu gücün ne kadar sürede sönümlenip biteceği

    [Header("Ekran Sınırları")]
    public Vector2 minBounds = new Vector2(-15, -10);
    public Vector2 maxBounds = new Vector2(15, 5);

    private bool isFishing = false;
    private float currentDownwardForce = 0f; // Arkada hesaplanan anlık ivme

    public void SetFishingMode(bool state)
    {
        isFishing = state;

        if (isFishing)
        {
            // Balık tutma BAŞLADIĞI ANDA kancaya aşağı yönlü devasa bir ivme ver
            currentDownwardForce = initialThrowForce;
        }
        else
        {
            // Mod kapanırsa gücü sıfırla ki bir dahaki sefere temiz başlasın
            currentDownwardForce = 0f;
        }
    }

    void Update()
    {
        if (!isFishing) return;

        // 1. ADIM: İVMEYİ SÖNÜMLENDİR (AddForce Hissi)
        // Eğer kancada hala bir fırlatma gücü varsa, onu zamanla eritip sıfırla
        if (currentDownwardForce > 0)
        {
            currentDownwardForce -= forceDecaySpeed * Time.deltaTime;

            // Eğer eksiye düşerse tam sıfırda sabitle (Böylece yerçekimi tamamen biter)
            if (currentDownwardForce < 0)
            {
                currentDownwardForce = 0f;
            }
        }

        // 2. ADIM: OYUNCU KONTROLÜ
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Y eksenindeki hız = Oyuncunun WASD gücü EKSİ anlık kalan düşme ivmesi
        float currentSpeedY = (moveY * moveSpeed) - currentDownwardForce;
        float currentSpeedX = (moveX * moveSpeed);

        // 3. ADIM: HAREKETİ UYGULA VE SINIRLA
        Vector3 newPos = transform.position + new Vector3(currentSpeedX, currentSpeedY, 0) * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        transform.position = newPos;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isFishing) return;

        Fish hitFish = other.GetComponent<Fish>();
        if (hitFish != null)
        {
            hitFish.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}