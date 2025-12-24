using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject ballCane;

    [SerializeField]
    private GameObject ResumePopup;
    
    private float shootSpeed = 5f;
    private bool isFirst = true;

    //projectile 관련
    public float maxSpeedRate = 10f;
    public int maxDamageType = 4;
    public int score = 0;
    public int ballCount = 1;
    public bool IsPaused { get; private set; }

    public Sprite[] BallSprites;
    public GameObject projectilePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


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

    public void CheckGameOver()
    {
        if (ballCount > 0)
            return;

        IsPaused = true;

        if (isFirst)
        {
            Time.timeScale = 0f;
            ResumePopup.SetActive(true);
            isFirst = false;
        }

        else
        {

        }
    }


}
