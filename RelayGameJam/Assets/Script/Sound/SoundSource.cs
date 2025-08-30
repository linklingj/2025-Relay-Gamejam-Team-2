using UnityEngine;

public class SoundSource : MonoBehaviour
{
    [System.Serializable]
    public struct AudioType
    {
        public string name;
        public AudioClip audio;
    }

    public AudioType[] SoundList;
    public SoundManager soundManager;
    public AudioSource m_Source;
    private string NowSoundname = "";
    public bool isLoop;

    public void PlaySound(string name)
    {
        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                m_Source.clip = SoundList[i].audio;
                m_Source.Play();
                NowSoundname = name;
            }
    }

    public void StopSound()
    {
        m_Source.Stop();
    }
}
