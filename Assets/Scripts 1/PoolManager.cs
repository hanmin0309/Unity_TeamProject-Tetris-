using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    public int spawnCnt;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
        Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;


        foreach (GameObject item in pools[index])
        {   

            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //새로 생성
        if (!select && spawnCnt < 20 )
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
            spawnCnt++; 
        }

        return select;
    }
}
