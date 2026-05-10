using Unity.VisualScripting;
using UnityEngine;

public class PMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    float inputX;
    
    bool isFacingRight = true;
    Animator anim;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Flip();
        AnimatorVariables();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        inputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        if(isFacingRight && inputX < 0f || !isFacingRight && inputX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void AnimatorVariables()
    {
        anim.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocity.x));
    }
}
