using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    public SoundSource BGM;
    public SoundSource EffectSound;
    
    // 필요 없는 부분이라 삭제함
    //private static SoundManager Instance = null;
    
    public float BGM_V;
    public float Effect_V;
    public void Awake()
    {
        // 현재 스테이지에 맞는 BGM 실행
        string sceneName = SceneManager.GetActiveScene().name;
        BGM.PlaySound(sceneName);
        
    }

    private void Start()
    {
        // BGM 사운드 초기화
        BGM.m_Source.volume = BGM_V;
        
        // Effect 사운드 초기화
        EffectSound.m_Source.volume = Effect_V;
        
    }

    public void AdjustVolume_BGM(float soundVol)   //BGM 조절
    {
        BGM_V = soundVol;
        BGM.m_Source.volume = BGM_V;
    }

    public void AdjustVolume_EffectSound(float soundVol)  //effect 조절
    {
        Effect_V = soundVol;
        EffectSound.m_Source.volume = Effect_V;
        
    }

    public void ChangeToStageSelectScene() // 사운드 조절 옵션 패널에서 선택치
    {
        FadeInFadeOutManager.Inst.FadeOut("StageSelect");
    }
}
