using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;

[Serializable]
public class DataToSave
{
    public string userName;
    public string userEmail;
    public int bestScore;
    public int enemyKill;
}

public class DataSaver : MonoBehaviour
{
    public static DataSaver instance;

    public DataToSave dts;
    public string userId;
    public string userName;
    public int bestScore;
    DatabaseReference dbRef;

    DataSaver ds;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("DataSaver instance initialized.");
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }

        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveDataFn()
    {
        Debug.Log("저장완료");
        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void LoadDataFn(Action onComplete = null)
    {
        StartCoroutine(LoadDataEnum(onComplete));
    }

    IEnumerator LoadDataEnum(Action onComplete = null)
    {
        var serverData = dbRef.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("process is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("process data found");

            dts = JsonUtility.FromJson<DataToSave>(jsonData);
            userName = dts.userName;
            bestScore = dts.bestScore;
        }
        else
        {
            print("no data found");
            dts = null;
        }

        onComplete?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
