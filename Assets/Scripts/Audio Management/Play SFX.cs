using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public static PlaySFX instance;
    public AudioSource walkSound; // loop
    public AudioSource fallSound;
    public AudioSource deathSound;
    public AudioSource respawnSound;
    public AudioSource platformSound; // loop
    public AudioSource switchSound;

    void Awake()
    {
        instance = this;
    }

    public void playWalk()
    {
        walkSound.Play();
    }

    public bool checkWalk()
    {
        return walkSound.isPlaying;
    }

    public void stopWalk()
    {
        walkSound.Stop();
    }

    public void playFall()
    {
        fallSound.Play();
    }

    public void playDeath()
    {
        deathSound.Play();
    }

    public void playRespawn()
    {
        respawnSound.Play();
    }

    public void playPlatform()
    {
        platformSound.Play();
    }

    public bool checkPlatform()
    {
        return platformSound.isPlaying;
    }

    public void stopPlatform()
    {
        platformSound.Stop();
    }

    public void playSwitch()
    {
        switchSound.Play();
    }
}