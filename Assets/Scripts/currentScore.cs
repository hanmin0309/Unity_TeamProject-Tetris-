using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class currentScore : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    private void Start()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }

        if (highScoreText == null)
        {
            highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        }

        UpdateScoreUI();
    }

    private void Update()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.gm.score.ToString();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + GameManager.gm.highScore.ToString();
        }
    }
}
