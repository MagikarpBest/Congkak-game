using System;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    SEEDSPAWN,

}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;  
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)]; // Play random sounds, prevent repetitive

        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("No clip asssigned/ clip Error");
            return;
        }
            
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] name = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, name.Length );
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = name[i];
        }
    }
#endif

    [Serializable]
    public class SoundList
    {
        public AudioClip[] Sounds { get => sounds; }

        [SerializeField] public string name;
        [SerializeField] private AudioClip[] sounds;
    }
}
