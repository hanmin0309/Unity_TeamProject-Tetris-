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
    public GameObject endPanel;
    public GameObject rankPanel;
    public Text endScoreText;
    public Text TitleText;
    public Text endBestScoreText;
    public Text endRankingText;
    public Text endRankingAllText;

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
        //highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScore = DataSaver.instance.bestScore;
        gamePlay = true;
        //24-08-21 Sound(SFX)효과음 편집
        //AudioManager.instance.PlayBgm(true);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Upgrade);

    }

    // Update is called once per frame
    void Update()
    {

        if (!gamePlay)
        {
            return;
        }

        if (score > highScore)
        {
            highScore = score;
            DataSaver.instance.bestScore = score;

            //PlayerPrefs.SetInt("HighScore", highScore); // 최고 스코어를 저장
        }

        if (health <= 0)
        {
            GameOver();
        }

        if (bossHealth <= 0)
        {
            GameEnd();
            GameOver();
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

    public void GameOver()
    {
        Debug.Log("game over");
        gamePlay = false;
        DataSaver.instance.dts.bestScore = highScore;
        DataSaver.instance.dts.enemyKill = kill;
        DataSaver.instance.SaveDataFn();

        OpenDeadPanel();
       
    }

    public void GameEnd()
    {

    }

    public void OpenDeadPanel()
    {
        endPanel.SetActive(true);
        RankingManager.instance.GetAllRank(endRankingAllText, endRankingText);
        endScoreText.text = "" + score;
        endBestScoreText.text = "" + highScore;
        //RankingManager.instance.GetUserRank();

    }

    public void CloseDeadPanel()
    {
        endPanel.SetActive(false);

    }

    public void OpenRankPanel()
    {
        rankPanel.SetActive(true);
    }
    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void CloseRankPanel()
    {
        rankPanel.SetActive(false);
    }

    public void Retry()
    {
        GameManager.gm.gamePlay = true;
        GameManager.gm.health = 100;
        SceneManager.LoadScene("Tetris_Fighter");
    }
}
