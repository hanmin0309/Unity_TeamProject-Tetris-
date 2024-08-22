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
    public int damage;
    public float speed;
}

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public int wave;
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
        if (!GameManager.gm.gamePlay)
        {
            Debug.Log("���� ����");
            return;
        }

        timer += Time.deltaTime;

        if (wave >= spawnData.Length)
        {
            // Debug.Log("��� ���� �Ϸ�");
            return; // �� �̻� ���� �����Ͱ� ������ �����մϴ�.
        }

        if (timer > spawnData[wave].spawnTime && enemyCnt <= spawnData[wave].enemyCnt && !GameManager.gm.bossTime && wave < spawnData.Length)
        {
            timer = 0;

            Spawn();
        }

    }

    void Spawn()
    {
        if (enemyCnt == spawnData[wave].enemyCnt)
        {
            if (wave == 1)
            {
                //������ȯ
                Debug.Log("������ȯ");
                GameObject enemy = GameManager.gm.pool.Get(1);
                enemy.transform.position = spawnPoint[1].position;
                GameManager.gm.bossTime = true;
            }

            enemyCnt = 0;
            Debug.Log(wave);
            wave++;

        }
        else
        {
            GameObject enemy = GameManager.gm.pool.Get(0);
            enemy.transform.position = spawnPoint[1].position;
            enemy.GetComponent<Enemy>().Init(spawnData[wave]);
            enemyCnt++;
            Debug.Log("�� ���� ��" + enemyCnt);
        }

    }

    private IEnumerator SpawnBossAfterDelay()
    {
        // ��� �ð� ���� ���
        yield return new WaitForSeconds(10f);

        // ���⼭ ���� ���� �ڵ带 ȣ���մϴ�.
    }
}


