using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton instance

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the SoundManager between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate SoundManagers
        }
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayMenuSound(AudioClip clip)
    {
        // Logic to play menu-specific sounds
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayGameplaySound(AudioClip clip)
    {
        // Logic to play gameplay-specific sounds
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}

