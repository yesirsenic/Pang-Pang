using UnityEngine;

public class SpeedUpBall : Ball
{
    public float speedUprate = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            float speed = collision.gameObject.GetComponent<Projectile>().speed;
            if (speed < GameManager.Instance.maxSpeedRate)
            {
                speed += speedUprate;
                collision.gameObject.GetComponent<Projectile>().speed = speed;
            }

            Destroy(gameObject);
        }
    }
}
