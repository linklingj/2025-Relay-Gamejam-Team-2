namespace Service
{
    public interface IPlayerService
    {
        public PlayerInfo playerInfo { get; }

        public void LoadPlayer();
        public void SavePlayer();   
    }
}