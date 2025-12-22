using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject ballCane;

    [SerializeField]
    private GameObject projectilePrefab;

    private float shootSpeed = 5f;



    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    //임시이고 아마 뒤에 애니메이션 사용할 수 있음.
    IEnumerator ShootCoroutine()
    {
        ballCane.SetActive(true);

        yield return new WaitForSeconds(1f);

        Vector3 spawnPos = ballCane.transform.GetChild(0).position;

        // 발사체 소환
        GameObject projectile =
            Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // 각도 랜덤 선택
        float[] angles = { 210f, 225f, 240f };

        float angle = angles[Random.Range(0, angles.Length)];

        float rad = angle * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        //발사
        rb.linearVelocity = dir * shootSpeed;

        yield return new WaitForSeconds(1f);

        ballCane.SetActive(false);
    }


}
