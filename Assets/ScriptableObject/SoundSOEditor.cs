#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using static SoundManager;

// This script customizes the Inspector for SoundsSO
[CustomEditor(typeof(SoundSO))]
public class SoundsSOEditor : Editor
{
    // Runs when the SoundsSO asset is selected in the Inspector
    private void OnEnable()
    {
        // Get reference to the sounds array inside SoundsSO
        ref SoundList[] soundList = ref ((SoundSO)target).sounds;

        // If there is no array, stop
        if (soundList == null)
        {
            return;
        }

        // Get all enum names from SoundType (SEEDSPAWN, JUMP, etc)
        string[] names = Enum.GetNames(typeof(SoundType));
        bool differentSize = names.Length != soundList.Length;

        Dictionary<string, SoundList> sounds = new();

        // If size changed, backup existing sounds by name
        if (differentSize)
        {
            for (int i = 0; i < soundList.Length; ++i)
            {

                sounds.Add(soundList[i].name, soundList[i]);
            }
        }

        Array.Resize(ref soundList, names.Length);

        // Loop through every enum name
        for (int i = 0; i < soundList.Length; i++)
        {
            string currentName = names[i];
            soundList[i].name = currentName;
            if (soundList[i].volume == 0) soundList[i].volume = 1;

            // If size was changed, try to restore old values
            if (differentSize)
            {
                // If old data exists with the same name
                if (sounds.ContainsKey(currentName))
                {
                    SoundList current = sounds[currentName];

                    // Copy back old values
                    UpdateElement(ref soundList[i], current.volume, current.sounds, current.mixer);
                }
                else
                {
                    // Otherwise, assign default empty values
                    UpdateElement(ref soundList[i], 1, new AudioClip[0], null);
                }

                // Local helper function to update element
                static void UpdateElement(ref SoundList element, float volume, AudioClip[] sounds, AudioMixerGroup mixer)
                {
                    element.volume = volume;
                    element.sounds = sounds;
                    element.mixer = mixer;
                }
            }
        }
    }
}
#endif