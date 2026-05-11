using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public TownManager.TownZone zone;

    private TownManager _manager;

    private void Awake()
    {
        _manager = GetComponentInParent<TownManager>();
        if (_manager == null)
            _manager = FindObjectOfType<TownManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _manager.OnZoneEnter(zone, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _manager.OnZoneExit(zone);
    }
}