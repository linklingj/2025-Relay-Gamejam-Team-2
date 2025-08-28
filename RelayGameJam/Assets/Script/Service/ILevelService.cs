using System;

namespace Service
{
    /// <summary>
    /// 스테이지 관리를 제공
    /// </summary>
    public interface ILevelService
    {
        public event Action<Stage> OnStageChange;

        public void SetStage(string stage);

        public Stage GetStage();
    }
}