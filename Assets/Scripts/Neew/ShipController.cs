using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Ship Settings")]
    public float shipSpeed = 5f;
    public Transform exitPoint;
    public GameObject player;

    public bool isPlayerNeerDoor = false;
    public bool isPlayerInside = false;    

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (isPlayerInside)
        {
            MoveShip();
        }

    }

    private void EnterShip()
    {
        isPlayerInside = true;
        player.SetActive(false);
    }

    private void ExitShip()
    {
        isPlayerInside = false;
        player.transform.position = exitPoint.position;
        player.SetActive(true);
        rb.linearVelocity = Vector2.zero; // Gemi Kaymasın diye
    }

    private void MoveShip()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(moveX, moveY) * shipSpeed;

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
