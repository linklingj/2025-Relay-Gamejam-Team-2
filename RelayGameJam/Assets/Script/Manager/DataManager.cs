using System;
using System.Collections.Generic;
using System.IO;
using CardData;
using UnityEngine;
using VInspector;
using Service;

[Serializable]
public class PlayerInfo
{
    // 플레이어 정보
    public Guid playerID;
    public string playerName;
    
    // Level
    public int Hp;
    
    // 카드 해금 정보
    public List<int> cardList;
    
    // 덱 정보
    public List<int> cardDeck;

    public PlayerInfo()
    {
        playerID = Guid.NewGuid();
        playerName = "new Player";

        Hp = 40;

        cardList = new();

        // 시작용 스킬
        cardList = new()
        {
            0,
            1
        };

        cardDeck = new()
        {
            0,
            1
        };
    }

    public void UnlockCard(int id)
    {
        if (!cardList.Contains(id))
            cardList.Add(id);
    }
}

public class DataManager : SingletonDontDestroyOnLoad<DataManager>, IPlayerService
{
    public string path;
    public PlayerInfo playerInfo { get; private set; }
    private string fileName = "GameData.json";


    protected override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.persistentDataPath, fileName);
        LoadPlayer();
    }

    public void LoadPlayer()
    {
        if (!File.Exists(path))
        {
            CreatePlayer();
            SavePlayer();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            playerInfo = JsonUtility.FromJson<PlayerInfo>(loadJson);
        }
    }
    
    public void SavePlayer()
    {
        string json = JsonUtility.ToJson(playerInfo, true);
        File.WriteAllText(path, json);
    }

    public void CreatePlayer()
    {
        playerInfo = new PlayerInfo();
    }

    [Button]
    public void Reset()
    {
        if (File.Exists(path))
        {
            File.Delete(path); // 파일 삭제
        }

        // 새 데이터로 초기화
        CreatePlayer();
        SavePlayer();
    }

    private void OnApplicationQuit() => SavePlayer();
    private void OnApplicationFocus(bool focus) => SavePlayer();
}
