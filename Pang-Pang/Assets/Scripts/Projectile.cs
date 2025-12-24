using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;

    public int damageType = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * rb.linearVelocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir = rb.linearVelocity.normalized;

        if (Mathf.Abs(dir.y) > 0.95f)
        {
            dir.x = Random.Range(-0.3f, 0.3f);
        }

        if (Mathf.Abs(dir.x) > 0.95f)
        {
            dir.y = Random.Range(0.3f, 0.6f);
        }

        dir.Normalize();
        rb.linearVelocity = dir * speed;

        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<Boss>().OnHit();

            GameManager.Instance.score += (int)Mathf.Pow(2, damageType);

            Debug.Log(GameManager.Instance.score);
        }

        if (collision.gameObject.tag == "Ground")
        {
            GameManager.Instance.ballCount--;
            GameManager.Instance.CheckGameOver();
            Destroy(gameObject);
        }
    }
}
