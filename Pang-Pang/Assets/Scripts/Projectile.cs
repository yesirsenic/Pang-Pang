using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
