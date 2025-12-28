using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damageType = 1;
    private Rigidbody2D rb;
    private bool isEnd = false;

    Camera cam;

    private float margin = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        speed += GameManager.Instance.timer * 0.2f;

        if(speed > GameManager.Instance.maxNormalSpeedRate)
        {
            speed = GameManager.Instance.maxNormalSpeedRate;
        }
    }

    private void Update()
    {
        DestoryByOutside();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 v = rb.linearVelocity.normalized;

        const float minComponent = 0.25f;

        if (Mathf.Abs(v.x) < minComponent)
        {
            v.x = Mathf.Sign(v.x == 0 ? Random.Range(-1f, 1f) : v.x) * minComponent;
        }

        if (Mathf.Abs(v.y) < minComponent)
        {
            v.y = Mathf.Sign(v.y == 0 ? 1f : v.y) * minComponent;
        }

        v.Normalize();
        rb.linearVelocity = v * speed;

        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<Boss>().OnHit();

            GameManager.Instance.score += (int)Mathf.Pow(2, damageType);

            Debug.Log(GameManager.Instance.score);
        }

        if (collision.gameObject.tag == "Ground")
        {
            if(!isEnd)
            {
                isEnd = true;
            }
            Destroy(gameObject);
            GameManager.Instance.OnCheckGameOver();
            
        }

        if(collision.gameObject.tag == "ReflectionBlock")
        {
            AudioManager.Instance.PlaySFX("ReBound");
        }

        if(collision.gameObject.tag =="Wall")
        {
            AudioManager.Instance.PlaySFX("WallBound");
        }
    }

    private void DestoryByOutside()
    {
        Vector3 v = cam.WorldToViewportPoint(transform.position);

        if (v.x < -margin || v.x > 1f + margin ||
            v.y < -margin || v.y > 1f + margin)
        {
            if (!isEnd)
            {
                isEnd = true;
            }
            Destroy(gameObject);
            GameManager.Instance.OnCheckGameOver();
        }
    }
}
