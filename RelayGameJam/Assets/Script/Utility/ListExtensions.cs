using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{

    public static void Shuffle<T>(this List<T> list)
    {
        int n = 0;
        while (n < list.Count)
        {
            int random = Random.Range(0, list.Count - 1);
            var temp = list[random];
            list[random] = list[n];
            list[n] = temp;
            n++;
        }
    }

    public static List<T> NonDupRandomT<T>(this List<T> list,int count) //중복 없는 랜덤 반환
    {
        HashSet<T> randomHashSet = new HashSet<T>();
        
        while (randomHashSet.Count < count)
        {
            int random = Random.Range(0, list.Count);
            randomHashSet.Add(list[random]);
        }

        return randomHashSet.ToList();
    }
    
    public static List<T> DupRandomT<T>(this List<T> list,int count)  //중복 있는 랜덤 반환
    {
        List<T> randomList = new List<T>();
        
        while (randomList.Count < count)
        {
            int random = Random.Range(0, list.Count);
            randomList.Add(list[random]);
        }

        return randomList;
    }
}
