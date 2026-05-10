using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SimpleWaterSurface : MonoBehaviour
{
    [Header("Surface Shape")]
    public int pointsCount = 32;
    public float width = 20f;
    public float surfaceY = 0f;

    [Header("Wave Physics")]
    public float tension = 0.02f;
    public float damping = 0.04f;
    public float spread = 0.05f;
    public int spreadIterations = 4;

    [Header("Interaction")]
    public LayerMask interactionMask;
    public float impactMultiplier = 0.4f;
    public float surfaceTriggerHeight = 0.5f;

    private LineRenderer _lineRenderer;
    private BoxCollider2D _trigger;
    private float[] _heights;
    private float[] _velocities;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _trigger = GetComponent<BoxCollider2D>();
        _trigger.isTrigger = true;
    }

    private void Start()
    {
        InitializeSurface();
        UpdateTrigger();
    }

    private void Update()
    {
        SimulateWaves();
        UpdateLineRenderer();
    }

    private void InitializeSurface()
    {
        pointsCount = Mathf.Max(2, pointsCount);
        _heights = new float[pointsCount];
        _velocities = new float[pointsCount];
        _lineRenderer.positionCount = pointsCount;
    }

    private void SimulateWaves()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            float displacement = _heights[i];
            float acceleration = -tension * displacement - _velocities[i] * damping;
            _velocities[i] += acceleration;
        }

        for (int i = 0; i < pointsCount; i++)
        {
            _heights[i] += _velocities[i] * Time.deltaTime;
        }

        for (int iter = 0; iter < spreadIterations; iter++)
        {
            for (int i = 1; i < pointsCount - 1; i++)
            {
                float leftDelta = spread * (_heights[i] - _heights[i - 1]);
                float rightDelta = spread * (_heights[i] - _heights[i + 1]);

                _velocities[i - 1] += leftDelta;
                _velocities[i + 1] += rightDelta;
            }
        }
    }

    private void UpdateLineRenderer()
    {
        float spacing = width / (pointsCount - 1f);
        float startX = -width * 0.5f;

        for (int i = 0; i < pointsCount; i++)
        {
            float x = startX + i * spacing;
            float y = surfaceY + _heights[i];
            _lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    private void UpdateTrigger()
    {
        _trigger.size = new Vector2(width, surfaceTriggerHeight);
        _trigger.offset = new Vector2(0f, surfaceY + surfaceTriggerHeight * 0.5f);
    }

    private void OnValidate()
    {
        if (pointsCount < 2) pointsCount = 2;
        if (width < 0.1f) width = 0.1f;
        if (surfaceTriggerHeight < 0.05f) surfaceTriggerHeight = 0.05f;

        if (_lineRenderer != null)
        {
            InitializeSurface();
            UpdateLineRenderer();
        }

        if (_trigger != null)
        {
            UpdateTrigger();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & interactionMask.value) == 0)
        {
            return;
        }

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        Vector2 localPos = transform.InverseTransformPoint(other.bounds.center);
        float x = Mathf.Clamp(localPos.x, -width * 0.5f, width * 0.5f);
        float velocity = rb.linearVelocity.y + Mathf.Abs(rb.linearVelocity.x) * 0.25f;
        AddWaveAtLocalX(x, velocity * impactMultiplier);
    }

    public void AddWaveAtWorldX(float worldX, float strength)
    {
        float localX = transform.InverseTransformPoint(new Vector3(worldX, 0f, 0f)).x;
        AddWaveAtLocalX(localX, strength);
    }

    private void AddWaveAtLocalX(float localX, float strength)
    {
        float t = Mathf.InverseLerp(-width * 0.5f, width * 0.5f, localX);
        int index = Mathf.Clamp(Mathf.RoundToInt(t * (pointsCount - 1)), 0, pointsCount - 1);
        _velocities[index] += strength;
    }
}

