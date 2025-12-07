using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    SEEDSPAWN,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundSO soundSO;
    private static SoundManager instance = null;
    private AudioSource audioSource;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }

    }

    public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
    {
        SoundList soundList = instance.soundSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)]; // Play random sounds, prevent repetitive

        if (source)
        {
            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;
            source.volume = volume * soundList.volume;
            source.Play();
        }
        else
        {
            instance.audioSource.outputAudioMixerGroup = soundList.mixer;
            instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
        }
    }


    [Serializable]
    public class SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;   
        public AudioClip[] sounds;
    }
}
