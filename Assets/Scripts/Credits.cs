using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private float gecisSuresi = 15f;
        [SerializeField] private string hedefSahneAdi = "MainMenu";
        private bool _gecisYapildi;

        private void Start()
        {
            StartCoroutine(SureSonraSahneGecisi());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HedefSahneyeGec();
            }
        }

        private IEnumerator SureSonraSahneGecisi()
        {
            yield return new WaitForSeconds(gecisSuresi);
            HedefSahneyeGec();
        }

        private void HedefSahneyeGec()
        {
            if (_gecisYapildi)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(hedefSahneAdi))
            {
                _gecisYapildi = true;
                SceneManager.LoadScene(hedefSahneAdi);
            }
            else
            {
                Debug.LogWarning("Hedef sahne adı boş. Credits scripti sahne geçişi yapmadı.");
            }
        }
    }
}
