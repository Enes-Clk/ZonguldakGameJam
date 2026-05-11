using System.Collections;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    [SerializeField] private int health = 4;
    [SerializeField] private float speed = 1f;
    SpriteRenderer sr;
    float damageCooldown = 0.5f; // Hasar alma sonrası bekleme süresi
    float directionChangeInterval = 2f;
    private Vector3 currentDirection;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        directionChangeInterval = 0f; // Hemen yön seçmeye başla
        currentDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * speed;
        damageCooldown = 0f;
    }

    void Update()
    {
        FishAI();
        if(Input.GetKeyDown(KeyCode.K))
        {
            currentDirection *= -1;
        }

        Flip();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hook"))
        {
            damageCooldown -= Time.deltaTime; // Cooldown'u azalt
            // Hasar verme işlemi için cooldown kontrolü
            if (damageCooldown <= 0f)
            {
                TakeDamage(1); // Kanca ile temas halinde hasar almaya devam eder
                StartCoroutine(DamageFish()); // Hasar alma efektini başlat
                damageCooldown = 0.5f; // Cooldown'u sıfırla
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            currentDirection *= -1;
            directionChangeInterval = 3f; // Sınır çarptığında yön değiştirme süresini sıfırla
            Debug.Log("Balık sınırla çarpıştı, yön değiştiriyor.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hook"))
        {
            damageCooldown = 0f; // Kanca temasından çıktıktan sonra cooldown'u sıfırla
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.FishCaught(); // Balık yakalandığını GameManager'a bildir
        Destroy(gameObject);
    }

    IEnumerator DamageFish()
    {
        // Hasar verme mantığı burada olacak
        sr.color = Color.red; // Örnek: Hasar aldığında kırmızıya dön
        yield return new WaitForSeconds(0.2f); // Kırmızı kalma
        sr.color = Color.white; // Rengi eski haline getir
        
    }

    void FishAI()
    {
        // Basit bir yapay zeka örneği: Balık rastgele hareket eder
        // Yön değiştirme süresi
        directionChangeInterval -= Time.deltaTime;
        if(directionChangeInterval <= 0f)
        {
            directionChangeInterval = 2f; // Yön değiştirme süresini sıfırla
            currentDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * speed;
        }
        
        transform.Translate(currentDirection * 0.1f * Time.deltaTime);
    }

    void Flip()
    {
        if (currentDirection.x > 0)
        {
            sr.flipX = false;
        }
        else if (currentDirection.x < 0)
        {
            sr.flipX = true;
        }
    }
}
