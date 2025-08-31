using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{

    // 효과음 설정
    public Slider BGM_Slider;
    public Slider Effect_Slider;
    
    // 패널이 활성화되었을 때, Slider의 값이 SoundManager의 값을 따르도록 설정
    private void OnEnable()
    {
        BGM_Slider.value = SoundManager.Inst.BGM_V;
        Effect_Slider.value = SoundManager.Inst.Effect_V;
    }
    
}
