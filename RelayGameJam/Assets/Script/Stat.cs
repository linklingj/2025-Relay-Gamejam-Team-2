using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    public delegate void OnValueChangeDelegate(float value);
    public event OnValueChangeDelegate OnValueChanged;
    private float maxValue;
    public float baseValue;
    public static Stat Create(float baseValue = 0)
    {
        var stat = new Stat
        {
            baseValue = baseValue,
            maxValue = baseValue
        };
        return stat;
    }

    public void AddValue(float value)
    {
        baseValue += value;
        baseValue = Mathf.Clamp(baseValue, 0, maxValue);
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