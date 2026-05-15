using System;
using UnityEngine;

public class ColliderDisableTrigger : MonoBehaviour
{
    [Header("Hedef Collider")]
    public Collider2D targetCollider;

    private Rigidbody2D rb;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!other.CompareTag("Player")) return;
        // if (targetCollider == null) return;

        targetCollider.enabled = false;

        if (other.gameObject.CompareTag("Tekne"))
        {
            rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        
        
    }
}