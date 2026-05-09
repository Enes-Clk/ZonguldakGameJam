using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [Header("Paneller (Inspector'dan Sürükle-Bırak)")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitConfirmationPanel;

    [Header("Sahne İsimleri")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string creditsSceneName = "CreditsScene";

    private void Start()
    {
        // Oyun başladığında panellerin kapalı olduğundan emin oluyoruz.
        // Bu sayede sahnede açık unutsan bile oyun başlarken düzelecektir.
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (quitConfirmationPanel != null) quitConfirmationPanel.SetActive(false);
    }

    private void Update()
    {
        // ESC tuşuna basıldığını kontrol ediyoruz
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Eğer Settings paneli açıksa, onu kapat
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            // Eğer Quit paneli açıksa, onu kapat (iptal et)
            else if (quitConfirmationPanel.activeSelf)
            {
                CloseQuitConfirmation();
            }
            // Ekranda hiçbir panel açık değilse Quit doğrulama panelini aç
            else
            {
                OpenQuitConfirmation();
            }
        }
    }

    // --- BUTON METOTLARI ---

    // Start Game butonuna atanacak metot
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Credits butonuna atanacak metot
    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    // Settings butonuna atanacak metot
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Settings'ten çıkış veya ESC için
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Quit butonuna atanacak metot (Doğrulama panelini açar)
    public void OpenQuitConfirmation()
    {
        quitConfirmationPanel.SetActive(true);
        
    }

    // Doğrulama panelindeki "Hayır/İptal" butonuna atanacak metot
    public void CloseQuitConfirmation()
    {
        quitConfirmationPanel.SetActive(false);
    }

    // Doğrulama panelindeki "Evet/Çık" butonuna atanacak metot
    public void ConfirmQuit()
    {
        Debug.Log("Oyundan çıkış yapılıyor..."); // Editörde görmek için
        Application.Quit(); // Build alındığında çalışır
    }
}