using UnityEngine;

public class JumpScareToggle : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    
    public void PlayScareSound()
    {
        if (JumpScareManagement.IsEnabled)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
    
}
