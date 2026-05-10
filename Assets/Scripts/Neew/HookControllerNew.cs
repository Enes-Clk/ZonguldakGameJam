using UnityEngine;

public class HookControllerNew : MonoBehaviour
{[Header("Kanca Ayarları")]
    public float hookSpeed = 5f; // Kancanın su altındaki hızı
    public float maxDistance = 10f; // Kanca gemiden en fazla ne kadar uzaklaşabilir?
    
    [Header("Referanslar")]
    public Transform rodTip; // İpin başlayacağı yer (Gemideki nokta)
    public LineRenderer lineRenderer; // İp çizicimiz
    public ShipController shipController; // Geminin kodu (Gemiyi durdurmak için)

    private Rigidbody2D rb;
    private bool isFishing = false; // Balık tutma modunda mıyız?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Oyun başladığında kancayı gizle
        gameObject.SetActive(false);
        lineRenderer.positionCount = 0; // İpi gizle
    }

    void Update()
    {
        // Oyuncu F tuşuna basarsa balık modunu aç/kapat
        // SADECE geminin içindeyken balık tutabilsin diye ShipController'ı kontrol ediyoruz.
        if (Input.GetKeyDown(KeyCode.F) && shipController.isPlayerInside)
        {
            isFishing = !isFishing;

            if (isFishing)
                StartFishing();
            else
                StopFishing();
        }

        // Eğer balık tutma modundaysak kancayı kontrol et
        if (isFishing)
        {
            MoveHook();
            DrawRope();
        }
    }

    private void MoveHook()
    {
        // Kanca WASD kontrolleri
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveX, moveY) * hookSpeed;
        
        // Kancanın gemiden (rodTip) çok fazla uzaklaşmasını engelliyoruz
        float distanceFromShip = Vector2.Distance(transform.position, rodTip.position);
        
        // Eğer kanca maksimum mesafeyi aştıysa ve daha da uzaklaşmaya çalışıyorsa hızı kes
        if (distanceFromShip > maxDistance && movement.magnitude > 0)
        {
            // Basitçe kancayı sınırda tutmak için merkeze doğru hafifçe çekebiliriz
            Vector2 directionToShip = (rodTip.position - transform.position).normalized;
            movement += directionToShip * hookSpeed; 
        }

        rb.linearVelocity = movement;
    }

    private void DrawRope()
    {
        // İpin başlangıcını gemideki noktaya, bitişini kancanın olduğu yere çiz
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, rodTip.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void StartFishing()
    {
        // Gemiyi durdur (Birazdan ShipController'a bu özelliği ekleyeceğiz)
        shipController.isPlayerInside = false; 

        // Kancayı görünür yap ve geminin yanına ışınla
        transform.position = rodTip.position;
        gameObject.SetActive(true);
    }

    private void StopFishing()
    {
        // Gemiyi tekrar hareket edebilir yap
        shipController.isPlayerInside = true;

        // Kancayı ve ipi gizle
        gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
        rb.linearVelocity = Vector2.zero;
    }

    // KANCA BALIĞA DEĞDİ Mİ?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Balık Yakalandı!");
            // Şimdilik balığı yok edelim. İleride buraya envanter veya skor kodu ekleyebilirsin.
            //damage fish
        }
    }
}