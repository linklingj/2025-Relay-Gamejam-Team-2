using TMPro;
using UnityEngine;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultPhrase;  //게임 클리어/ 실패 문구
    [SerializeField] private GameObject gameResult;
    [SerializeField] private float fadeDelay = 0.5f;
    public void setResultPhrase(string phrase)      //게임 클리어 실패 문구 설정 및 화면 표시
    {
        resultPhrase.text = phrase;
        gameResult.SetActive(true);
    }

    public void LoadMain()
    {
        FadeInFadeOutManager.Inst.FadeOut("Start", true, fadeDelay);
    }

    public void LoadStageScene()
    {
        FadeInFadeOutManager.Inst.FadeOut("StageSelect", true, fadeDelay);
    }
}
