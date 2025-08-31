
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class JumpScare : MonoBehaviour
{
    [SerializeField] private GameObject jumpScare;
    [SerializeField] private Sprite[] ImgArr;
    public float duration = 1f;
    public float moveDistance = 10f;
    public float strength = 10f;
    public int vibrato = 20;
    public bool randomness = true;
    Vector3 pos;
    
    #region Effect Sound
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] horrorClip;
    
    [SerializeField] private AudioClip cuteClip;
    
    #endregion

    private void Awake()
    {
        pos = jumpScare.transform.position;     //초기 위치 저장
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }
    public IEnumerator SurpriseAttack()     //점점 커지는 연출
    {
        //효과음 재생
        if (JumpScareManagement.IsEnabled)
        {
            SoundManager.Inst.EffectSound.PlaySound("다가오는 공포 효과음");
            audioSource.PlayOneShot(horrorClip[3]);
        }
        else
        {
            //무서운 거 방지
            audioSource.PlayOneShot(cuteClip);
        }

        SetDefault(0);

        float time = 0f;
        Vector3 startScale = Vector3.zero;      //초기 크기
        Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);      //목표 크기

        while(time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            jumpScare.transform.localScale = Vector3.Lerp(startScale, endScale, t);     //목표 크기 도달할 때까지 크기 키움
            yield return null;
        }

        //마지막 목표 크기 설정
        jumpScare.transform.localScale = endScale;
        jumpScare.SetActive(false);
    }

    public IEnumerator MentalAbsorption()
    {
        if (JumpScareManagement.IsEnabled)
        {
            //효과음 재생
            SoundManager.Inst.EffectSound.PlaySound("사라지는 공포 효과음");
            audioSource.PlayOneShot(horrorClip[2]);
        }
        else
        {
            //무서운 거 방지
            audioSource.PlayOneShot(cuteClip);            
        }
        

        SetDefault(1);

        Vector3 startPos = pos;

        jumpScare.transform.DOShakePosition(duration, strength, vibrato, 90, randomness);

        yield return new WaitForSeconds(1f);
        jumpScare.SetActive(false);
    }

    public IEnumerator SpirteSubjection()
    {
        
        if (JumpScareManagement.IsEnabled)
        {
            //효과음 재생
            SoundManager.Inst.EffectSound.PlaySound("짧은 공포 효과음");
            audioSource.PlayOneShot(horrorClip[1]);
        }
        else
        {
            //무서운 거 방지
            audioSource.PlayOneShot(cuteClip);            
        }
        
        SetDefault(2);

        Vector3 startPos = pos;

        jumpScare.transform.DOShakePosition(duration, strength, vibrato, 90, randomness);

        yield return new WaitForSeconds(1f);
        jumpScare.SetActive(false);
    }

    public IEnumerator Chaos()
    {
        if (JumpScareManagement.IsEnabled)
        {
            //효과음 재생
            SoundManager.Inst.EffectSound.PlaySound("다가오는 공포 효과음");
            audioSource.PlayOneShot(horrorClip[0]);
        }
        else
        {
            //무서운 거 방지
            audioSource.PlayOneShot(cuteClip);            
        }
        
        SetDefault(3);

        jumpScare.transform.DOShakePosition(duration, strength, vibrato, 90, randomness);
        yield return new WaitForSeconds(1f);
        jumpScare.SetActive(false);
    }

    public void SetDefault(int i)
    {
        Image temp = jumpScare.GetComponent<Image>();
        
        Debug.Log(PlayerPrefs.GetInt("JumpScare", 1));
        
        
        //점프 스케어 유무 (true - 발동)
        if (JumpScareManagement.IsEnabled)
        {
            temp.sprite = ImgArr[i];
        }
        else // 점프 스케어 방지 (false)
        {
            temp.sprite = ImgArr[i + 4];
        }
        
        
        jumpScare.SetActive(true);   //연출 표시
        jumpScare.transform.localScale = Vector3.one;
        jumpScare.transform.position = pos;
    }
}
