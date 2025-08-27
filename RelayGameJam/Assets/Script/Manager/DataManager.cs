using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VInspector;

[Serializable]
public class GameData
{

    
}
public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    string path;
    public GameData Data;
    private string fileName = "GameData.json";
    public bool isOpening = true;


    protected override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.persistentDataPath, fileName);
        JsonLoad();
    }

    private void Start()
    {
        
    }


    private void JsonLoad()
    {
        if (!File.Exists(path))
        {
            Data = new GameData();
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            Data = JsonUtility.FromJson<GameData>(loadJson);
        }
    }
    

    public void JsonSave()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(path, json);
    }

    [Button]
    public void Reset()
    {
        if (File.Exists(path))
        {
            File.Delete(path); // 파일 삭제
        }

        // 새 데이터로 초기화
        Data = new GameData();
        JsonSave();
    }

    private void OnApplicationQuit() => JsonSave();
    private void OnApplicationFocus(bool focus) => JsonSave();
}
