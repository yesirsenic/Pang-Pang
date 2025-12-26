using System.Collections;
using UnityEngine;


public class Boss : MonoBehaviour
{
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hitSprite;

    SpriteRenderer sr;

    Coroutine hitCoroutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnHit()
    {
        if(hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitRoutine());
        AudioManager.Instance.PlaySFX("BossDamage");
    }

    IEnumerator HitRoutine()
    {
        sr.sprite = hitSprite;

        yield return new WaitForSeconds(0.2f);

        sr.sprite = normalSprite;

        hitCoroutine = null;
    }






    
}
