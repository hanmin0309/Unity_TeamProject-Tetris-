using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    [Header("# Game control")]
    public float gameTime;
    public bool bossTime = false;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int health;
    public int maxHealth;
    public int kill;
    public int score;
    public int highScore;


    [Header("# Game object")]
    public PoolManager pool;
    public Rigidbody2D player;
    public Rigidbody2D bossEndPoint;
    public Text enemyKillText;
    public Text scoreText;
    public Text highScoreText;

    // Start is called before the first frame update

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject); // GameManager가 씬 전환 시 사라지지 않도록 함
        }
        else
        {
            Destroy(gameObject); // 이미 gm이 존재하면 새로 생성된 GameManager를 삭제
        }

    }
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // 최고 스코어를 저장
        }

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }


        SetPlayerInfo();
    }

    public void SetPlayerInfo()
    {
        enemyKillText.text = "적 처치 수:" + kill;
        scoreText.text = "점수 : " + score;
        highScoreText.text = "최고 점수 : " + highScore;
    }


}
