using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WavyWater : MonoBehaviour
{
    [Header("Wave Settings")]
    public int nodeCount = 20;
    public float totalWidth = 10f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float speed = 1f;
    public float noiseStrength = 0.1f;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.positionCount = nodeCount;

        // Apply alpha gradient across the LineRenderer length.
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(Color.white, 0f);
        colorKeys[1] = new GradientColorKey(Color.white, 1f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(0.1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);

        gradient.SetKeys(colorKeys, alphaKeys);
        _lineRenderer.colorGradient = gradient;
    }

    private void Update()
    {
        if (_lineRenderer.positionCount != nodeCount)
        {
            _lineRenderer.positionCount = nodeCount;
        }

        float spacing = nodeCount > 0 ? totalWidth / nodeCount : 0f;

        for (int i = 0; i < nodeCount; i++)
        {
            float x = i * spacing;
            float wave = Mathf.Sin(x * frequency + Time.time * speed) * amplitude;
            float noise = Mathf.PerlinNoise(x * frequency, Time.time * speed) * noiseStrength;
            float y = wave + noise;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }
}
