using UnityEngine;

public class HookControllerNew : MonoBehaviour
{[Header("Kanca Ayarları")]
    public float hookSpeed = 5f; // Kancanın su altındaki hızı
    public float returnSpeed = 8f; // Kancanın geri dönüş hızı
    public float maxDistance = 10f; // Kanca gemiden en fazla ne kadar uzaklaşabilir?
    
    [Header("Kamera Ayarları")]
    public float cameraFollowSpeed = 5f; // Kameranın takip hızı
    
    [Header("Referanslar")]
    public Transform rodTip; // İpin başlayacağı yer (Gemideki nokta)
    public LineRenderer lineRenderer; // İp çizicimiz
    public ShipController shipController; // Geminin kodu (Gemiyi durdurmak için)

    private Rigidbody2D rb;
    private bool isFishing = false; // Balık tutma modunda mıyız?
    private bool isReturning = false; // Kanca geri dönüyor mu?
    private Vector3 originalCameraPos; // Orijinal kamera pozisyonu

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Oyun başladığında kancayı gizle
        gameObject.SetActive(true);
        lineRenderer.positionCount = 0; // İpi gizle
    }

    void Update()
    {
        // Oyuncu F tuşuna basarsa balık modunu aç/kapat
        // Geminin içindeyken VEYA balık tutarken F tuşu çalışsın
        if (Input.GetKeyDown(KeyCode.F) && (shipController.isPlayerInside || isFishing))
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
            FollowHookWithCamera();
        }
        
        // Kanca geri dönüyorsa
        if (isReturning)
        {
            ReturnHook();
            DrawRope();
            FollowHookWithCamera();
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
        shipController.isFishing = true; // Balık tutma modunu işaretle
        shipController.FreezeShip(); // Gemiyi sabitle

        // Kancayı görünür yap ve geminin yanına ışınla
        transform.position = rodTip.position;
        
        // Orijinal kamera pozisyonunu kaydet
        originalCameraPos = Camera.main.transform.position;
    }

    private void StopFishing()
    {
        // Kancayı geri çek
        isFishing = false;
        isReturning = true;
        rb.linearVelocity = Vector2.zero; // Hareketi durdur
        
        shipController.isFishing = false; // Balık tutma modunu kapat
    }
    
    private void ReturnHook()
    {
        // Kancayı rod tip konumuna doğru hareket ettir
        Vector2 directionToRod = (rodTip.position - transform.position).normalized;
        rb.linearVelocity = directionToRod * returnSpeed;
        
        // Kanca rod tip'e yakın mı kontrol et
        float distanceToRod = Vector2.Distance(transform.position, rodTip.position);
        if (distanceToRod < 0.3f)
        {
            // Kanca rod tip'e ulaştı
            transform.position = rodTip.position;
            rb.linearVelocity = Vector2.zero;
            isReturning = false;
            lineRenderer.positionCount = 0; // İpi gizle
            
            // Kamerayı orijinal pozisyonuna geri döndür
            Camera.main.transform.position = originalCameraPos;
            
            // Gemiyi tekrar hareket edebilir yap
            shipController.isPlayerInside = true;
            shipController.UnfreezeShip(); // Gemiyi serbest bırak
        }
    }
    
    private void FollowHookWithCamera()
    {
        // Kamerayı hook'un pozisyonunu doğrudan takip etmesi için ayarla (titremeyi önlemek için)
        Vector3 hookPos = transform.position;
        Vector3 targetCameraPos = new Vector3(hookPos.x, hookPos.y, originalCameraPos.z);
        Camera.main.transform.position = targetCameraPos;
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