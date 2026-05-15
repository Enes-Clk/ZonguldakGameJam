using UnityEngine;
using UnityEngine.SceneManagement;

public class CamFollow : MonoBehaviour
{
    public static CamFollow Instance { get; private set; }
    public Transform target;
    public Transform playerTarget;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smooth = 0.125f;

    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;  // sahne yüklenince çağır
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // bellek sızıntısını önle
    }

    // Her sahne yüklendiğinde player'ı yeniden bul
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
            target = playerTarget;
        }
    }

    private void FixedUpdate()
    {
        // Null kontrolü — destroy edilmiş referanslara erişimi engelle
        if (target == null) return;

        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref _velocity, smooth);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetToPlayer()
    {
        target = playerTarget;
    }
}