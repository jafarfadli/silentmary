using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public string tagToCheck = "Game";
    public static BGMusic Instance;
    
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance.tagToCheck == tagToCheck)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance.DestroyMusic();   
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