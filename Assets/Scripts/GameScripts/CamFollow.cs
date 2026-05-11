using UnityEngine;

public class CamFollow : MonoBehaviour
{
	public static CamFollow Instance { get; private set; }
    public Transform target;
    public Transform playerTarget;
	public Vector3 offset = new Vector3(0, 0, -10);
	public float smooth = 0.125f;

	Vector3 velocity = Vector3.zero;

	void Start()
	{
		if(playerTarget == null)
		{
			playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void FixedUpdate() //If player movement code runs in Update, this method should also run in LateUpdate to avoid jittering
	{
		Vector3 movePosition = target.position + offset;
		transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, smooth);
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
