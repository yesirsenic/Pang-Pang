using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject ballCane;

    [SerializeField]
    private GameObject ResumePopup;

    [SerializeField]
    private GameObject ResultPopup;

    [SerializeField]
    private GameObject scoreNumberRenderer_Score;

    [SerializeField]
    private GameObject scoreNumberRenderer_BestScore;

    private float shootSpeed = 5f;
    private bool isFirst = true;
    private int bestScore = 0;

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

    private void OnResultPopup()
    {
        DeathManager.Instance.DeathCount++;

        if (DeathManager.Instance.DeathCount >=3)
        {
            AdsManager.Instance.ShowInterstitialAd();
            DeathManager.Instance.DeathCount = 0;
        }

        Time.timeScale = 0f;
        BestScoreSave();
        ResumePopup.SetActive(false);
        ResultPopup.SetActive(true);
        scoreNumberRenderer_Score.GetComponent<ScoreNumberRenderer>().SetScore(score);
        scoreNumberRenderer_BestScore.GetComponent<ScoreNumberRenderer>().SetScore(bestScore);

    }

    private void BestScoreSave()
    {
        int best = PlayerPrefs.GetInt("BestScore");

        if(best < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        bestScore = PlayerPrefs.GetInt("BestScore");
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
            OnResultPopup();
        }
    }

    public void OnClickWatchAd()
    {
        AdsManager.Instance.ShowRewardedAd(
        onCompleted: () =>
        {
            //광고 전부 시청시
            ResumePopup.SetActive(false);
            IsPaused = false;
            Time.timeScale = 1f;
            StartCoroutine(ShootCoroutine());
        },
        onFailed: () =>
        {
            OnResultPopup();
        }
    );
    }

    public void NotWatchAd()
    {
        OnResultPopup();
    }









}
