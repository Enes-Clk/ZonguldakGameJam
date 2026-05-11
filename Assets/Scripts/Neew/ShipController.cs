using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Ship Settings")]
    public float shipSpeed = 5f;
    public Transform exitPoint;
    public GameObject player;

    public bool isPlayerNeerDoor = false;
    public bool isPlayerInside = false;
    public bool isFishing = false; // Balık tutma modunda mıyız?    

    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalConstraints = rb.constraints; // Orijinal constraints'ı kaydet
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(isPlayerNeerDoor && !isPlayerInside)
            {
                EnterShip();
            }
            else if(isPlayerInside)
            {
                ExitShip();
            }
        }

        if (isPlayerInside && !isFishing)
        {
            MoveShip();
        }

    }

    private void EnterShip()
    {
        isPlayerInside = true;
        player.SetActive(false);
        
        // Kamerayı tekneye odakla
        if (Camera.main != null && Camera.main.GetComponent<CamFollow>() != null)
        {
            Camera.main.GetComponent<CamFollow>().target = transform;
        }
    }

    private void ExitShip()
    {
        isPlayerInside = false;
        player.transform.position = exitPoint.position;
        player.SetActive(true);
        rb.linearVelocity = Vector2.zero; // Gemi Kaymasın diye
        
        // Kamerayı player'a geri odakla
        if (Camera.main != null && Camera.main.GetComponent<CamFollow>() != null)
        {
            Camera.main.GetComponent<CamFollow>().target = player.transform;
        }
    }

    private void MoveShip()
    {
        float moveX = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveX, 0) * shipSpeed;

    }

    public void FreezeShip()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnfreezeShip()
    {
        rb.constraints = originalConstraints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNeerDoor = true;
            Debug.Log("Tekneyi sürmek için E'ye bas.");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNeerDoor = false;
            Debug.Log("Tekneyi sürmek için E'ye bas.");
        }
    }
}
