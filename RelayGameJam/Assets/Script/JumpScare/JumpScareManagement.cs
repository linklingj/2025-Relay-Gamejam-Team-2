using System;
using UnityEngine;
using UnityEngine.UI;

public class JumpScareManagement : MonoBehaviour
{
    // 다른 스크립트에서 접근
    public static bool IsEnabled { get; private set; }

    public Toggle jumpScareToggle;
    
    private void Awake()
    {
        // 게임 시작 시 PlayerPrefs에서 불러옴
        IsEnabled = PlayerPrefs.GetInt("JumpScare", 1) == 1;
    }
    
    private void OnEnable()
    {
        // UI가 활성화될 때마다 저장된 값으로 토글 상태를 갱신합니다.
        jumpScareToggle.isOn = IsEnabled;
    }

    public void ChangeJumpScareMode(bool isOn)
    {
        // 토글값을 static 메모리에 반영
        IsEnabled = isOn;
        
        PlayerPrefs.SetInt("JumpScare", IsEnabled ? 1 : 0);
        PlayerPrefs.Save(); // 실제 저장소에 데이터 저장
        
    }
}
