using System;
using UnityEngine;

public class UnitDataManager : MonoBehaviour
{
   private void Awake()
   {
       UnitData.Data.Load();
   }
   
   
}
