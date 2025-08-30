using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioMixerManager : MonoBehaviour
{
    // 음량 전체를 총괄하는 클래스
    [SerializeField] private AudioMixer audioMixer;

    // 각 슬라이더를 Inspector에서 할당
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider effectSlider;

    public TMP_Text masterData;
    public TMP_Text bgmData;
    public TMP_Text effectData;
    
    // UI 슬라이더와 연동될 볼륨 값 (0.0f ~ 1.0f)
    private float _masterVolume = 1.0f;
    private float _bgmVolume = 1.0f;
    private float _effectVolume = 1.0f;
    
    private void Awake()
    {
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        _effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);
        
    }

    private void Start()
    {
        // UI 슬라이더의 값을 불러온 값으로 설정
        masterSlider.value = _masterVolume;
        bgmSlider.value = _bgmVolume;
        effectSlider.value = _effectVolume;
        
        // 오디오 믹서의 값도 불러온 값으로 설정
        // (슬라이더 값이 바뀌어야만 Set 함수가 호출되므로, 시작할 때도 한번 호출해줘야 합니다)
        SetMasterVolume(_masterVolume);
        SetBGMVolume(_bgmVolume);
        SetEffectVolume(_effectVolume);
    }

    private void OnEnable()
    {
        // UI 슬라이더의 값을 불러온 값으로 설정
        masterSlider.value = _masterVolume;
        bgmSlider.value = _bgmVolume;
        effectSlider.value = _effectVolume;
    }

    private void Update()
    {
        masterData.text = masterSlider.value.ToString("F2") + "f";
        bgmData.text = bgmSlider.value.ToString("F2") + "f";
        effectData.text = effectSlider.value.ToString("F2") + "f";
    }

    
    public void SetMasterVolume(float volume)
    {
        _masterVolume = volume;
        audioMixer.SetFloat("Master", Mathf.Log10(volume > 0 ? volume : 0.0001f) * 20);
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
    }

    public void SetBGMVolume(float volume)
    {
        _bgmVolume = volume;
        audioMixer.SetFloat("BGM", Mathf.Log10(volume > 0 ? volume : 0.0001f) * 20);
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
    }

    public void SetEffectVolume(float volume)
    {
        _effectVolume = volume;
        audioMixer.SetFloat("Effect", Mathf.Log10(volume > 0 ? volume : 0.0001f) * 20);
        PlayerPrefs.SetFloat("EffectVolume", _effectVolume);
    }
    
    public void SaveTotalVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("EffectVolume", _effectVolume);
        PlayerPrefs.Save();
    }
    
    private void OnApplicationQuit()
    {
        SaveTotalVolume();
    }
}
