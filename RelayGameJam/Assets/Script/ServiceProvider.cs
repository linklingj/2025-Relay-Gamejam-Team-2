using Service;

public class ServiceProvider : SingletonDontDestroyOnLoad<ServiceProvider>
{
    public ILevelService levelService { get; private set; }

    public void Register(ILevelService instance)
    {
        levelService ??= instance;
    }

    public void Unregister(ILevelService instance)
    {
        if (levelService == instance)
        {
            levelService = null;
        }
    }
}
