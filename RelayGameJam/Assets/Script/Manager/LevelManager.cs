using System;
using Service;
using UnityEngine;

namespace CardData
{
    public class LevelManager : SingletonDontDestroyOnLoad<LevelManager>, ILevelService
    {
        [SerializeField] private string stagePath = "stages/";
        private Stage _stageData;
        
        /// <summary>
        /// 스테이지 변경 이벤트
        /// </summary>
        public event Action<Stage> OnStageChange;

        /// <summary>
        /// 스테이지 전환
        /// </summary>
        /// <param name="stage">해당하는 스테이지 객체명</param>
        public void SetStage(string stage)
        {
            var newStage = Resources.Load<Stage>(stagePath + stage);

            if (!newStage)
            {
                Debug.Log("Invalid stage Name: " + stage);
                return;
            }
            
            _stageData = newStage;
            OnStageChange?.Invoke(_stageData);
        }

        /// <summary>
        /// 스테이지 정보 제공
        /// </summary>
        /// <returns>현재 스테이지 정보</returns>
        public Stage GetStage()
        {
            return _stageData;
        }
    }
}