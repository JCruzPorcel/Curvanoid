using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoundHandler : MonoBehaviour
{
    [SerializeField] private ButtonSound[] sounds;

    private Dictionary<SoundName, ButtonSound> soundDictionary = new Dictionary<SoundName, ButtonSound>();

    void Awake()
    {
        foreach (ButtonSound sound in sounds)
        {
            if (sound.button != null)
            {
                soundDictionary[sound.clickSoundName] = sound;

                if (sound.playHoverSound)
                {
                    EventTrigger trigger = sound.button.gameObject.GetComponent<EventTrigger>();
                    if (trigger == null)
                        trigger = sound.button.gameObject.AddComponent<EventTrigger>();

                    EventTrigger.Entry entryHover = new EventTrigger.Entry();
                    entryHover.eventID = EventTriggerType.PointerEnter;
                    entryHover.callback.AddListener((eventData) =>
                    {
                        AudioManager.Instance.PlaySoundOnObject(sound.button.gameObject, sound.hoverSoundName);
                    });
                    trigger.triggers.Add(entryHover);
                }

                if (sound.playClickSound)
                {
                    sound.button.onClick.AddListener(() => { AudioManager.Instance.Play(sound.clickSoundName); });
                }
            }
        }
    }

    [System.Serializable]
    public class ButtonSound
    {
        public string name;
        public SoundName hoverSoundName;
        public SoundName clickSoundName;
        public Button button;
        public bool playHoverSound;
        public bool playClickSound;
    }
}
