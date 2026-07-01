using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicSlider : MonoBehaviour
{
    public Slider musicSlider;
    // [SerializeField] private TextMeshProUGUI sliderText;

    void Start()
    {
        musicSlider.value = AudioManager.instance.GetMusicVolume();
        musicSlider.onValueChanged.AddListener(OnVolumeChanged);
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    void OnVolumeChanged(float newValue)
    {
        AudioManager.instance.SetMusicVolume(newValue);
    }

    // public void sliderChange(float value)
    // {
    //     float localValue = value * 100;
    //     sliderText.text = localValue.ToString("0") + "%";
    // }
}