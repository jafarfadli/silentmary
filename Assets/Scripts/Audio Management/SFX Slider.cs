using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SFXSlider : MonoBehaviour
{
    public Slider sfxSlider;
    // [SerializeField] private TextMeshProUGUI sliderText;

    void Start()
    {
        sfxSlider.value = AudioManager.instance.GetSFXVolume();
        sfxSlider.onValueChanged.AddListener(OnVolumeChanged);
        AudioManager.instance.SetSFXVolume(sfxSlider.value);
    }

    void OnVolumeChanged(float newValue)
    {
        AudioManager.instance.SetSFXVolume(newValue);
    }
    
    // public void sliderChange(float value)
    // {
    //     float localValue = value * 100;
    //     sliderText.text = localValue.ToString("0") + "%";
    // }
}