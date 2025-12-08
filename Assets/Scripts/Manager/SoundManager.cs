using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    SEEDSPAWN,
    MAINMUSIC
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundSO soundSO;
    private static SoundManager instance = null;
    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            sfxSource = GetComponent<AudioSource>();

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Destroyed duplicate");
            Destroy(gameObject); // prevent duplicates
        }

    }

    public static void PlayMusic(SoundType sound, AudioSource source = null, float volume = 1)
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
            // Bandaid fix
            instance.musicSource.outputAudioMixerGroup = soundList.mixer;
            instance.musicSource.clip = randomClip;
            instance.musicSource.volume = volume * soundList.volume;
            instance.musicSource.loop = true;
            instance.musicSource.Play();
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
            instance.sfxSource.outputAudioMixerGroup = soundList.mixer;
            instance.sfxSource.PlayOneShot(randomClip, volume * soundList.volume);
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
