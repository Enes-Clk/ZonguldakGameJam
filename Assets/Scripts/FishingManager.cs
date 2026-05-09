using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform ropeOrigin;
    public Transform hookTransform;
    public LineRenderer lineRenderer;
    public HookController hookController;

    [Header("Animasyon Ayarları")]
    public float returnSpeed = 15f; // İpin geri sarılma hızı (Inspector'dan ayarlanabilir)

    private bool isFishingMode = false;
    private bool isReturning = false; // Kancanın geri dönme durumunu takip eder

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // E tuşuna basıldığında VE kanca halihazırda geri dönmüyorsa çalışır
        // (Böylece kanca dönerken oyuncu spamlarsa bug oluşmaz)
        if (Input.GetKeyDown(KeyCode.E) && !isReturning)
        {
            ToggleFishingMode();
        }

        // 1. Durum: Normal balık tutma
        if (isFishingMode)
        {
            DrawRope();
        }
        // 2. Durum: İp geri sarılıyor (Animasyon evresi)
        else if (isReturning)
        {
            // Kancayı kendi pozisyonundan, gemideki başlangıç noktasına doğru returnSpeed hızıyla çek
            hookTransform.position = Vector3.MoveTowards(hookTransform.position, ropeOrigin.position, returnSpeed * Time.deltaTime);
            DrawRope(); // Dönerken ipin çizilmeye devam etmesi gerekir

            // Kanca gemiye ulaştıysa (aralarındaki mesafe çok çok kısaldıysa) işlemi bitir
            if (Vector3.Distance(hookTransform.position, ropeOrigin.position) < 0.05f)
            {
                hookTransform.position = ropeOrigin.position; // Tam yerine oturt
                isReturning = false;                          // Dönüş bitti
                lineRenderer.enabled = false;                 // İpi gizle

                Debug.Log("Kanca gemiye döndü. Gemi tekrar hareket edebilir.");
                // TODO: Gemi hareket scriptini burada aktif edebilirsin
            }
        }
    }

    void ToggleFishingMode()
    {
        isFishingMode = !isFishingMode;

        if (isFishingMode)
        {
            Debug.Log("Balık tutma modu AKTİF.");
            hookTransform.position = ropeOrigin.position; // Atış başlangıç noktası
            hookController.SetFishingMode(true);          // WASD kontrolünü ver
            lineRenderer.enabled = true;                  // İpi göster

            // TODO: Gemi hareket scriptini burada devre dışı bırak
        }
        else
        {
            Debug.Log("Kanca geri toplanıyor...");
            hookController.SetFishingMode(false); // WASD kontrolünü anında kes! (Yoksa dönerken oyuncu direnebilir)
            isReturning = true;                   // Geri dönüş animasyonunu başlat
        }
    }

    void DrawRope()
    {
        lineRenderer.SetPosition(0, ropeOrigin.position);
        lineRenderer.SetPosition(1, hookTransform.position);
    }
}