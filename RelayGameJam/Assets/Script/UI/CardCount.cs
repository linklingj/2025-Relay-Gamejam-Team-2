using TMPro;
using UnityEngine;

public class CardCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardCntTxt;

    public void setCardCount(int count)     //카드 수 설정
    {
        cardCntTxt.text = "남은 카드 수: "+count.ToString();
    }
}
