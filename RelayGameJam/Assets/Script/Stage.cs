using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameWave
{
    public List<int> enemies;
}

[CreateAssetMenu(fileName = "NewWave", menuName = "GameplayData/Stage Data")]
public class Stage : ScriptableObject
{
    public string name;
    
    public int maxCardCount = 7;
    
    public int maxMana = 3;
    
    public List<GameWave> Waves = new();
}

