using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("Dönüş ve Görsel Ayarları")]
    public float turnSpeedDegrees = 360f;
    public float spriteRotationOffset = -90f;

    [Header("Ekran Sınırları")]
    public Vector2 minBounds = new Vector2(-15, -10);
    public Vector2 maxBounds = new Vector2(15, 5);

    [Header("İp (Çizim) Ayarları")]
    public LineRenderer lineRenderer;

    public bool isFishing = false;
    private Vector3 currentDirection;

    public void SetFishingMode(bool state)
    {
        isFishing = state;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = state;
            if (state) lineRenderer.positionCount = 2;
        }

        if (isFishing) currentDirection = Vector3.down;
    }

    void Update()
    {
        // YENİ EKLENEN KISIM: Eğer balık tutma modunda değilsek, kanca zorla teknede kalsın!
        if (!isFishing)
        {
            if (FishingManager.Instance != null && FishingManager.Instance.ropeOrigin != null)
            {
                transform.position = FishingManager.Instance.ropeOrigin.position; // Sürekli teknede tut
                transform.rotation = Quaternion.Euler(0, 0, spriteRotationOffset); // İptal olunca düzelt
            }
            return;
        }

        float currentSpeed = FishingManager.Instance.GetCurrentSpeed();
        minBounds.y = FishingManager.Instance.GetCurrentMaxDepth();
        if (FishingManager.Instance.ropeOrigin != null) maxBounds.y = FishingManager.Instance.ropeOrigin.position.y;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(moveX, moveY, 0).normalized;

        if (inputDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            float nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeedDegrees * Time.deltaTime);
            currentDirection = new Vector3(Mathf.Cos(nextAngle * Mathf.Deg2Rad), Mathf.Sin(nextAngle * Mathf.Deg2Rad), 0);
        }

        Vector3 newPos = transform.position + currentDirection * currentSpeed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);
        transform.position = newPos;

        if (currentDirection != Vector3.zero)
        {
            float visualAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, visualAngle + spriteRotationOffset);
        }

        UpdateFishingLine();
    }

    private void UpdateFishingLine()
    {
        if (lineRenderer == null || !isFishing) return;
        lineRenderer.SetPosition(0, FishingManager.Instance.ropeOrigin.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isFishing) return;
        Fish hitFish = other.GetComponent<Fish>();
        if (hitFish != null) hitFish.TakeDamage(FishingManager.Instance.GetCurrentDamage() * Time.deltaTime);
    }
}