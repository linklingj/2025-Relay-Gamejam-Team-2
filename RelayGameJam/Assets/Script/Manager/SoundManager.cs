using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    public SoundSource BGM;
    public SoundSource EffectSound;
    private static SoundManager Instance = null;
    public int BGM_V = 5;
    public int Effect_V = 5;
    public void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;      //ÇØ´ç ¾ÀÀÇ ºê±Ý play
        BGM.PlaySound(sceneName);
    }
    public void AdjustVolume_BGM(int SoundVol)          //bgm º¼·ý Á¶Àý
    {
        float v = (float)SoundVol / 10;
        BGM.m_Source.volume = v;
        BGM_V = SoundVol;
    }

    public void AdjustVolume_EffectSound(int SoundVol)  //effect º¼·ý Á¶Àý
    {
        float v = (float)SoundVol / 10;
        EffectSound.m_Source.volume = v;
        Effect_V = SoundVol;
    }
}
