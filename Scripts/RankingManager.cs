using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Database;



[System.Serializable]
public class UserScore
{
    public string userId;
    public string userName;
    public int bestScore;

    public UserScore(string userId, string userName, int score)
    {
        this.userId = userId;
        this.userName = userName;
        this.bestScore = score;
    }
}


public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    DatabaseReference databaseReference;
    public string userId; // 현재 유저의 ID
    public int userRank;
    public int bestScore; // 자신의 최고 점수
    public Text rankText;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // GameManager가 씬 전환 시 사라지지 않도록 함
        }
        else
        {
            Destroy(gameObject); // 이미 gm이 존재하면 새로 생성된 GameManager를 삭제
        }
    }
    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //GetUserRank();
    }

    public void GetUserRank()
    {
        databaseReference.Child("users").OrderByChild("bestScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                List<UserScore> allScores = new List<UserScore>();
                foreach (DataSnapshot child in snapshot.Children)
                {
                    string userId = child.Key;
                    string userName = child.Child("userName").Value.ToString();
                    int bestScore = int.Parse(child.Child("bestScore").Value.ToString());
                    allScores.Add(new UserScore(userId, userName, bestScore));
                }

                // 점수를 기준으로 내림차순 정렬
                allScores.Sort((a, b) => b.bestScore.CompareTo(a.bestScore));

                userRank = 0;
                // 유저의 랭크 찾기
                for (int i = 0; i < allScores.Count; i++)
                {
                    if (allScores[i].userId == userId)
                    {
                        userRank = i + 1;
                        Debug.Log("All User: " + allScores[i]);
                        bestScore = allScores[i].bestScore;
                        Debug.Log("Your Rank: " + userRank);
                        Debug.Log("Your Best Score: " + bestScore);
                        rankText.text = ""+ userRank;
                        break;
                    }
                }

                if (userRank == 0)
                {
                    Debug.Log("Your score is not ranked.");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve scores: " + task.Exception);
            }
        });
    }
}

