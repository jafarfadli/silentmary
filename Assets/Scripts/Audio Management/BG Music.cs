using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public string tagToCheck = "Game";
    public static BGMusic instance;
    
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance.tagToCheck == tagToCheck)
            {
                Destroy(gameObject);
            }
            else
            {
                instance.DestroyMusic();   
            }
        }
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    public void DestroyMusic()
    {
        Destroy(gameObject);
    }
}