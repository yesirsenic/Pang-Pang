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

    [SerializeField]
    private GameObject tutorial;

    private float shootSpeed = 5f;
    private bool isFirst = true;
    private int bestScore = 0;

    //projectile 관련
    public float maxSpeedRate = 100f;
    public float maxNormalSpeedRate = 80f;
    public int maxDamageType = 4;
    public int score = 0;
    public int timer = 0;

    //ball 관련
    public float spawnAndDestroyRate = 4f;
    public float minSpawnAndDestroyRate = 2f;
    public bool IsPaused { get; private set; }

    public Sprite[] BallSprites;
    public GameObject projectilePrefab;
    public GameObject projectileCollection;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        projectileCollection = new GameObject("ProjectileCollection");
    }


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (PlayerPrefs.GetInt("TutorialShown") == 0)
        {
            tutorial.SetActive(true);
            return;
        }



        StartCoroutine(ShootCoroutine());

        StartCoroutine(CountEverySecond());
    }

    IEnumerator ShootCoroutine()
    {
        ballCane.SetActive(true);

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX("FireSpell");

        Vector3 spawnPos = ballCane.transform.GetChild(0).position;

        // 발사체 소환
        GameObject projectile =
            Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        projectile.transform.SetParent(projectileCollection.transform);

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

    IEnumerator CountEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;

            spawnAndDestroyRate -= 0.05f * timer;

            if(spawnAndDestroyRate < minSpawnAndDestroyRate)
            {
                spawnAndDestroyRate = minSpawnAndDestroyRate;
            }
        }
    }

    private void OnResultPopup()
    {

#if !UNITY_WEBGL
        DeathManager.Instance.DeathCount++;

        if (DeathManager.Instance.DeathCount >= 2)
        {
            AdsManager.Instance.ShowInterstitialAd();
            DeathManager.Instance.DeathCount = 0;
        }   
#endif


        AudioManager.Instance.PlaySFX("Result");
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

    IEnumerator CheckGameOverFlow()
    {
        yield return null;

        if(projectileCollection.transform.childCount == 0)
        {
            CheckGameOver();
        }
    }

    public void CheckGameOver()
    {

        IsPaused = true;

#if UNITY_WEBGL
        isFirst = false;
#endif


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

    public void TutorialOffAndPlay()
    {
        PlayerPrefs.SetInt("TutorialShown", 1);
        PlayerPrefs.Save();

        tutorial.SetActive(false);

        StartCoroutine(ShootCoroutine());
    }

    public void OnCheckGameOver()
    {
        StartCoroutine(CheckGameOverFlow());
    }









}
