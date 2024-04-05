using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public enum SoundName
{
    NoSound,
    Music_MainMenu,
    Music_Level_0,
    Music_Level_1,
    Music_Level_2,
    Music_Level_3,
    UI_HoverButton,
    UI_HoverLevelButton,
    UI_ClickButton,
    UI_ClickLevelButton,
    UI_ClickBackButton,
    UI_ClickExitButton,
    UI_ContinueButton,
    UI_SaveButton,
    SFX_BallImpact,
    SFX_DestroyBrick,
    SFX_GameOver,
    SFX_StartGame,
}

public class AudioManager : MonoBehaviour
{   
    public static AudioManager Instance { get; private set; }

    [SerializeField] Sound[] sounds;
    private Dictionary<SoundName, AudioSource> audioSources = new Dictionary<SoundName, AudioSource>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            GameObject obj = new GameObject(sound.name);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.outputAudioMixerGroup = sound.mixergroup;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.playOnAwake = sound.playOnAwake;
            audioSources[sound.soundName] = source;

            if (sound.playOnAwake)
                Play(sound.soundName);

        }
    }

    public void Play(SoundName soundName)
    {
        if (soundName == SoundName.NoSound)
            return;

        if (!audioSources.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        audioSources[soundName].Play();
    }

    public void Stop(SoundName soundName)
    {
        if (audioSources.ContainsKey(soundName))
            audioSources[soundName].Stop();

    }

    public void PlaySoundOnObject(GameObject obj, SoundName soundName)
    {
        if (obj == null)
        {
            Debug.LogWarning("GameObject is null!");
            return;
        }

        AudioSource audioSource = obj.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = obj.AddComponent<AudioSource>();
        }

        if (!audioSources.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        audioSource.clip = audioSources[soundName].clip;
        audioSource.outputAudioMixerGroup = audioSources[soundName].outputAudioMixerGroup;
        audioSource.volume = audioSources[soundName].volume;
        audioSource.pitch = audioSources[soundName].pitch;
        audioSource.loop = audioSources[soundName].loop;

        audioSource.Play();
    }
}

[Serializable]
public class Sound
{
    public string name;
    public SoundName soundName;
    public AudioClip clip;
    public AudioMixerGroup mixergroup;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;
}
