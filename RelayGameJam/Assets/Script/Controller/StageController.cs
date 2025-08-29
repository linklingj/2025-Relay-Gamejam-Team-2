using System.Collections.Generic;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class StageController : MonoBehaviour, ISceneService
{
    [SerializeField] private float fadeDelay = 0.5f;
    
    [Foldout("Stages")]
    private List<Stage> _stages;
    public List<Vector2> buttonPositions;
    
    public GameObject buttonPrefab;
    
    private ILevelService _levelService;
    void Start()
    {
        _levelService = LevelManager.Inst;
        _stages = _levelService.GetAllStage();

        LoadStageButton();
    }

    /// <summary>
    /// UI에 스테이지 로드하기
    /// </summary>
    private void LoadStageButton()
    {
        for (int i = 0; i < buttonPositions.Count && i < _stages.Count; i++)
        {
            // 스테이지 버튼 생성
            GameObject button = Instantiate(buttonPrefab, transform);
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = buttonPositions[i];
            button.GetComponentInChildren<TMP_Text>().text = _stages[i].stageName;

            // 스테이지 선택 
            var index = i;
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(_stages[index].stageName));
        }
    }
    
    public void LoadScene(string sceneName)
    {
        if (_levelService == null)
        {
            Debug.Log("LevelService Error");
            return;
        }
        
        _levelService.SetStage(sceneName);
        FadeInFadeOutManager.Inst.FadeOut("BattleScene", true, fadeDelay);
    }

    public void Quit()
    {
        FadeInFadeOutManager.Inst.FadeOut(0, true, fadeDelay);
    }
}
