using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image hpImage;
    [SerializeField] private TextMeshProUGUI hpTxt;
    private float maxHp;
    private Camera _camera;
    private float offsetY = -50f;

    public void Init(float maxHp,Transform trans) //초기 최대체력 설정
    {
        this.maxHp = maxHp;
        _camera = Camera.main;
        SetPos(trans);
        SetHpFillAmount(maxHp);
    }

    private void SetPos(Transform trans)
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(trans.position);
        screenPos += new Vector3(0, offsetY);
        transform.position = screenPos;
    }
    
    public void SetHpFillAmount(float curHp) //이미지의 FillAmount변경
    {
        hpImage.fillAmount = curHp/maxHp; //비율
        SetHpTxt(curHp); //현재 체력 텍스트 변경
    }

    private void SetHpTxt(float curHp)
    {
       hpTxt.text = curHp.ToString()+"/"+maxHp.ToString();
    }
}
