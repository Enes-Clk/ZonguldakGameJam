using UnityEngine;

public class KancaSesKontrol : MonoBehaviour
{
    private AudioSource sesKaynagi;

    [Header("Ses Dosyaları")]
    public AudioClip suyaGirmeSesi;
    public AudioClip balikYakalamaSesi;

    void Start()
    {
        // Hook objesindeki AudioSource bileşenini otomatik olarak bulup tanımlar
        sesKaynagi = GetComponent<AudioSource>();
    }

    // Kanca suya girdiğinde diğer kodlardan bu fonksiyonu çağıracağız
    public void SuyaGirmeSesiniCal()
    {
        if (suyaGirmeSesi != null)
        {
            sesKaynagi.PlayOneShot(suyaGirmeSesi); // PlayOneShot seslerin üst üste binmesini sağlar
        }
    }

    // Balık kancaya çarptığında diğer kodlardan bu fonksiyonu çağıracağız
    public void BalikYakalamaSesiniCal()
    {
        if (balikYakalamaSesi != null)
        {
            sesKaynagi.PlayOneShot(balikYakalamaSesi);
        }
    }
}