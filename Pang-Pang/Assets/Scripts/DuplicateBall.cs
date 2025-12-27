using UnityEngine;

public class DuplicateBall : Ball
{
    public float angle = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            AudioManager.Instance.PlaySFX("ItemSound");

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            Vector2 originalVelocity = rb.linearVelocity;
            float speed = originalVelocity.magnitude;
            Vector2 direction = originalVelocity.normalized;

            Vector2 dirClockwise = RotateVector(direction, angle);

            Vector2 dirCounter = RotateVector(-dirClockwise, angle);

            SpawnBall(dirClockwise, speed, GameManager.Instance.projectilePrefab); 
            SpawnBall(dirCounter, speed, GameManager.Instance.projectilePrefab);

            Destroy(gameObject);
            
        }
    }

    void SpawnBall(Vector2 dir, float speed, GameObject ball)
    {
        GameObject newBall = Instantiate(ball, transform.position, Quaternion.identity);
        Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * speed;

        newBall.transform.SetParent(GameManager.Instance.projectileCollection.transform);
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float x = cos * v.x - sin * v.y;
        float y = sin * v.x + cos * v.y;

        return new Vector2(x, y);
    }
}
