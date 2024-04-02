using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundName
{
    Music_MainMenu,
    SFX_BallImpact,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] Sound[] sounds;

    private Dictionary<SoundName, Sound> soundDictionary = new Dictionary<SoundName, Sound>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.mixergroup;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;

            soundDictionary[sound.soundName] = sound;
        }
    }

    public void Play(SoundName soundName)
    {
        if (!soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        Debug.Log($"Playing sound: {soundName}");
        soundDictionary[soundName].source.Play();
    }

    public void Stop(SoundName soundName)
    {
        if (!soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return;
        }

        Debug.Log($"Stopping sound: {soundName}");
        soundDictionary[soundName].source.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public SoundName soundName;
    public AudioClip clip;
    public AudioMixerGroup mixergroup;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;

    [HideInInspector]
    public AudioSource source;
}
