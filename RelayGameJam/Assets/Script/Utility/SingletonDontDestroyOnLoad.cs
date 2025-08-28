using UnityEngine;

public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : SingletonDontDestroyOnLoad<T>
{
    private static T inst;

    public static T Inst
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<T>();

                if (inst == null)
                {
                    Debug.LogError($"Unable to find an inst of {typeof(T)}. Make sure there is at least one active object of type {typeof(T)} in the scene.");
                }
            }

            return inst;
        }
    }

    protected virtual void Awake()
    {
        if (inst == null)
        {
            inst = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
