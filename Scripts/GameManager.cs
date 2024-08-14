using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int score;
    public int highScore;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    public PoolManager pool;
    public Rigidbody2D player;
    // Start is called before the first frame update

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject); // GameManager�� �� ��ȯ �� ������� �ʵ��� ��
        }
        else
        {
            Destroy(gameObject); // �̹� gm�� �����ϸ� ���� ������ GameManager�� ����
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
            PlayerPrefs.SetInt("HighScore", highScore); // �ְ� ���ھ ����
        }

        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        } 
    }
}
