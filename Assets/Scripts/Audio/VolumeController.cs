using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private const string SFXVolumeKey = "SFXVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private void OnEnable()
    {
        LoadVolume();
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        SetVolume(volume, SFXVolumeKey, sfxMixerGroup);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        SetVolume(volume, MusicVolumeKey, musicMixerGroup);
    }

    private void SetVolume(float volume, string volumeKey, AudioMixerGroup mixerGroup)
    {
        float adjustedVolume;

        if (volume > 0f)
        {
            adjustedVolume = Mathf.Log10(volume) * 20;
        }
        else
        {
            // Si el volumen es 0, establece el volumen del mezclador en el valor mínimo posible (generalmente -80 dB)
            adjustedVolume = -80f;
        }

        mixerGroup.audioMixer.SetFloat(volumeKey, adjustedVolume);
        PlayerPrefs.SetFloat(volumeKey, volume);
    }

    public void LoadVolume()
    {
        LoadVolume(SFXVolumeKey, sfxSlider, sfxMixerGroup);
        LoadVolume(MusicVolumeKey, musicSlider, musicMixerGroup);
    }

    private void LoadVolume(string volumeKey, Slider slider, AudioMixerGroup mixerGroup)
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            float volume = PlayerPrefs.GetFloat(volumeKey);
            slider.value = volume;
            float adjustedVolume = Mathf.Log10(volume) * 20;
            mixerGroup.audioMixer.SetFloat(volumeKey, adjustedVolume);
        }
    }
}
