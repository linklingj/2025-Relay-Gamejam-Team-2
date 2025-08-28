using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    public delegate void OnValueChangeDelegate(float value);
    public event OnValueChangeDelegate OnValueChanged;
    public float maxValue;
    bool isMax = false;
    public float baseValue;
    public static Stat Create(float baseValue = 0)
    {
        var stat = new Stat
        {
            baseValue = baseValue,
        };
        return stat;
    }

    public void SetMaxValue(float value)
    {
        isMax = true;
        maxValue = value;
    }

    public void AddMaxValue(float value)
    {
        isMax = true;
        maxValue += value;
    }

    public void AddValue(float value)
    {
        baseValue += value;
        if(isMax) baseValue = Mathf.Clamp(baseValue, 0, maxValue);
       
        ChangeValueHandler();
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
        ChangeValueHandler();
    }
    
    private void ChangeValueHandler()
    {
        OnValueChanged?.Invoke(baseValue);
    }
}