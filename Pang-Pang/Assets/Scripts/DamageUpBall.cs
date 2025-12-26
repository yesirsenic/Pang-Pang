using UnityEngine;

public class DamageUpBall : Ball
{
    private int damageUpRate = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            AudioManager.Instance.PlaySFX("ItemSound");

            if (collision.gameObject.GetComponent<Projectile>().damageType < GameManager.Instance.maxDamageType)
            {
                collision.gameObject.GetComponent<Projectile>().damageType += damageUpRate;
                Sprite sp = SwitchSprite(collision.gameObject.GetComponent<Projectile>().damageType);
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = sp;
            }

            Destroy(gameObject);
        }
    }

    Sprite SwitchSprite(int type)
    {
        Sprite ren = null;

        switch(type)
        {
            case 1:
                ren = gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.BallSprites[0];
                break;
            case 2:
                ren = gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.BallSprites[1];
                break;
            case 3:
                ren = gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.BallSprites[2];
                break;
            case 4:
                ren = gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.BallSprites[3];
                break;
        }

        return ren;
    }
}
