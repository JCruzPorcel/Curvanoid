using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Utils.JCruzPorcel.AudioManager;

namespace Utils.JCruzPorcel
{
    public class AudioManager : MonoBehaviour
    {
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
            SFX_BallImpact,
            SFX_DestroyBrick,
        }

        public static AudioManager Instance { get; private set; }

        [SerializeField] Sound[] sounds;
        private Dictionary<SoundName, Queue<AudioSource>> soundPool = new Dictionary<SoundName, Queue<AudioSource>>();

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
                Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();
                int queueSize = sound.queueSize > 0 ? sound.queueSize : 1;
                for (int i = 0; i < queueSize; i++)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.clip = sound.clip;
                    source.outputAudioMixerGroup = sound.mixergroup;
                    source.volume = sound.volume;
                    source.pitch = sound.pitch;
                    source.loop = sound.loop;
                    source.playOnAwake = sound.playOnAwake;
                    source.enabled = false;
                    audioSourcePool.Enqueue(source);
                }
                soundPool[sound.soundName] = audioSourcePool;
            }
        }

        public void Play(SoundName soundName)
        {
            if (soundName == SoundName.NoSound)
            {
                return;
            }

            if (!soundPool.ContainsKey(soundName))
            {
                Debug.LogWarning($"Sound: {soundName} not found!");
                return;
            }

            Queue<AudioSource> audioSourceQueue = soundPool[soundName];

            if (audioSourceQueue.Count == 0)
            {
                Debug.LogWarning($"No available AudioSource for sound: {soundName}");
                return;
            }

            AudioSource source = audioSourceQueue.Dequeue();
            source.enabled = true;

            if (!source.loop)
                StartCoroutine(PlaySoundCoroutine(source, soundName));
        }

        private IEnumerator PlaySoundCoroutine(AudioSource source, SoundName soundName)
        {
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            source.enabled = false;
            soundPool[soundName].Enqueue(source);
        }

        public void Stop(SoundName soundName)
        {
            foreach (var source in soundPool[soundName])
            {
                source.Stop();
                source.enabled = false;
            }
        }
    }

    [System.Serializable]
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
        [Range(1, 20)] public int queueSize = 1;

        [HideInInspector]
        public AudioSource source;
    }
}
