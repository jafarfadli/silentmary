using UnityEngine;

public class CutsceneSFX : MonoBehaviour
{
    public static CutsceneSFX instance;
    public AudioSource paintingSound;
    public AudioSource brakeSound;
    public AudioSource trunkSound;

    void Awake()
    {
        instance = this;
    }
    public void playPainting()
    {
        paintingSound.Play();
    }

    public void playBrake()
    {
        brakeSound.Play();
    }

    public void playTrunk()
    {
        trunkSound.Play();
    }
}