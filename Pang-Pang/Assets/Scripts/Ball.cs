using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyCorutine());
    }

    IEnumerator DestroyCorutine()
    {
        yield return new WaitForSeconds(GameManager.Instance.spawnAndDestroyRate);

        Destroy(gameObject);
    }
}
