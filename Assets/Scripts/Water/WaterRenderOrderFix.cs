using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WaterRenderOrderFix : MonoBehaviour
{
    [Header("Sorting")]
    public string sortingLayerName = "Water";
    public int sortingOrder = 0;

    [Header("Material Queue")]
    public bool overrideRenderQueue = true;
    public int renderQueue = 2500;

    [Header("Apply")]
    public bool applyEveryFrame = true;

    private MeshRenderer _meshRenderer;
    private Material _materialInstance;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        Apply();
    }

    private void LateUpdate()
    {
        if (applyEveryFrame)
        {
            Apply();
        }
    }

    private void Apply()
    {
        if (_meshRenderer == null)
        {
            return;
        }

        _meshRenderer.sortingLayerName = sortingLayerName;
        _meshRenderer.sortingOrder = sortingOrder;

        if (overrideRenderQueue)
        {
            if (_materialInstance == null && _meshRenderer.material != null)
            {
                _materialInstance = _meshRenderer.material;
            }

            if (_materialInstance != null)
            {
                _materialInstance.renderQueue = renderQueue;
            }
        }
    }
}
