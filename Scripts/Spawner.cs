using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int enemyCnt;
    public int spriteType;
    public int health;
    public float speed;
}


public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    int enemyCnt;
    float timer;
    //bool bossTime;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.gm.gameTime / 10f), spawnData.Length - 1);


        if (timer > spawnData[0].spawnTime && enemyCnt <= spawnData[0].enemyCnt && !GameManager.gm.bossTime)
        {
            Debug.Log(level);
            timer = 0;
       

            Spawn();
        }
        else
        {
            

        }
    }

    void Spawn()
    {
        if (enemyCnt == spawnData[0].enemyCnt)
        {
            //보스소환
            Debug.Log("보스소환");
            GameObject enemy = GameManager.gm.pool.Get(1);
            enemy.transform.position = spawnPoint[1].position;
            enemyCnt = 0;
            GameManager.gm.bossTime = true;
        }
        else
        {
            GameObject enemy = GameManager.gm.pool.Get(0);
            enemy.transform.position = spawnPoint[1].position;
            enemy.GetComponent<Enemy>().Init(spawnData[0]);
            enemyCnt++;
            Debug.Log("적 생성 수"+ enemyCnt);
        }

    }
}


