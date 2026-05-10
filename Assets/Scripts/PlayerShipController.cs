using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public enum PlayerState { Walking, Steering, Fishing }

    [Header("State")]
    public PlayerState currentState = PlayerState.Walking;

    [Header("Player")]
    public Rigidbody2D playerRb;
    public float playerSpeed = 5f;
    public GameObject playerVisual;

    [Header("Ship")]
    public Rigidbody2D shipRb;
    public float shipSpeed = 4f;

    [Header("Cameras")]
    public GameObject vcamShip;
    public GameObject vcamHook;

    [Header("UI Prompts")]
    public GameObject hookPromptCanvas;
    public GameObject steerPromptCanvas;

    [Header("Interaction")]
    public bool isNearSteerZone;
    public bool isNearHookZone;
    public float hookReturnTolerance = 0.05f;

    private void Update()
    {
        if (currentState == PlayerState.Walking)
        {
            if (isNearHookZone && Input.GetKeyDown(KeyCode.E))
            {
                EnterFishing();
            }
            else if (isNearSteerZone && Input.GetKeyDown(KeyCode.E))
            {
                EnterSteering();
            }
        }

        if (currentState == PlayerState.Fishing)
        {
            TryExitFishingWhenHookReturns();
        }
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Walking)
        {
            ApplyHorizontalMovement(playerRb, playerSpeed);
        }
        else if (currentState == PlayerState.Steering)
        {
            ApplyHorizontalMovement(shipRb, shipSpeed);
        }
        else
        {
            StopHorizontalMovement(playerRb);
            StopHorizontalMovement(shipRb);
        }
    }

    private void ApplyHorizontalMovement(Rigidbody2D rb, float speed)
    {
        if (rb == null) return;

        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
    }

    private void StopHorizontalMovement(Rigidbody2D rb)
    {
        if (rb == null) return;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void EnterSteering()
    {
        currentState = PlayerState.Steering;
        SetPlayerVisualActive(false);
        SetCameras(true, false);
    }

    private void EnterFishing()
    {
        currentState = PlayerState.Fishing;
        SetPlayerVisualActive(false);
        SetCameras(false, true);

        if (FishingManager.Instance != null && FishingManager.Instance.hookController != null)
        {
            FishingManager.Instance.hookController.SetFishingMode(true);
        }
    }

    private void ExitToWalking()
    {
        currentState = PlayerState.Walking;
        SetPlayerVisualActive(true);
        SetCameras(true, false);

        if (FishingManager.Instance != null && FishingManager.Instance.hookController != null)
        {
            FishingManager.Instance.hookController.SetFishingMode(false);
        }
    }

    private void TryExitFishingWhenHookReturns()
    {
        if (FishingManager.Instance == null || FishingManager.Instance.hookTransform == null || FishingManager.Instance.ropeOrigin == null)
        {
            return;
        }

        float distance = Vector2.Distance(FishingManager.Instance.hookTransform.position, FishingManager.Instance.ropeOrigin.position);
        if (distance <= hookReturnTolerance)
        {
            ExitToWalking();
        }
    }

    private void SetPlayerVisualActive(bool active)
    {
        if (playerVisual != null)
        {
            playerVisual.SetActive(active);
        }
    }

    private void SetCameras(bool shipActive, bool hookActive)
    {
        if (vcamShip != null) vcamShip.SetActive(shipActive);
        if (vcamHook != null) vcamHook.SetActive(hookActive);
    }

    private void SetPromptState(GameObject prompt, bool active)
    {
        if (prompt != null)
        {
            prompt.SetActive(active);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("SteerZone"))
        {
            isNearSteerZone = true;
            SetPromptState(steerPromptCanvas, currentState == PlayerState.Walking);
        }
        else if (other.CompareTag("HookZone"))
        {
            isNearHookZone = true;
            SetPromptState(hookPromptCanvas, currentState == PlayerState.Walking);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SteerZone"))
        {
            isNearSteerZone = false;
            SetPromptState(steerPromptCanvas, false);
        }
        else if (other.CompareTag("HookZone"))
        {
            isNearHookZone = false;
            SetPromptState(hookPromptCanvas, false);
        }
    }
}

