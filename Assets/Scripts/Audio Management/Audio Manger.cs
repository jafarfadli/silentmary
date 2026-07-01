using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer masterMixer;
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    void Awake()
    {
        instance = this;
    }

    public void SetMusicVolume(float volume)
    {
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;

        masterMixer.SetFloat(MusicKey, dB);

        PlayerPrefs.SetFloat(MusicKey, volume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicKey, 0.8f);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;

        masterMixer.SetFloat(SFXKey, dB);

        PlayerPrefs.SetFloat(SFXKey, volume);
        PlayerPrefs.Save();
    }
    
    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXKey, 0.8f);
    }
}