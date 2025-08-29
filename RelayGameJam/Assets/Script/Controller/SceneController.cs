using Service;
using UnityEngine;

public class SceneController :  MonoBehaviour, ISceneService
{
    public float fadeDelay = 0.5f;
    public void LoadScene(string sceneName)
    {
        FadeInFadeOutManager.Inst.FadeOut(sceneName, true, fadeDelay);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
