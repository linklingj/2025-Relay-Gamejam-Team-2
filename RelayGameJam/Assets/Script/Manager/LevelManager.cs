using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;

public class LevelManager : SingletonDontDestroyOnLoad<LevelManager>, ILevelService
{
    public string stagePath = "stages/";
    private Stage _stageData;
    
    /// <summary>
    /// 스테이지 변경 이벤트
    /// </summary>
    public event Action<Stage> OnStageChange;

    void Start()
    {
        if (ServiceProvider.Inst)
        {
            ServiceProvider.Inst.Register(this);
        }
        #if DEBUG
        SetStage("DEBUG");
        #endif 
    }

    /// <summary>
    /// 스테이지 전환
    /// </summary>
    /// <param name="stage">해당하는 스테이지 객체명</param>
    public void SetStage(string stage)
    {
        var list = GetAllStage();
        Stage stageData = list.Find(st => st.stageName == stage);
        if (stageData != null)
        {
            _stageData = stageData;
            OnStageChange?.Invoke(_stageData);
        }
    }

    /// <summary>
    /// 스테이지 정보 제공
    /// </summary>
    /// <returns>현재 스테이지 정보</returns>
    public Stage GetStage()
    {
        return _stageData;
    }
    
    /// <summary>
    /// 스테이지 리스트 제공
    /// </summary>
    /// <returns>스테이지 목록 반환</returns>
    public List<Stage> GetAllStage()
    {
        List<Stage> stages = Resources.LoadAll<Stage>(stagePath).ToList();
        return stages;
    }
}