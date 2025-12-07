using UnityEngine;
using static SoundManager;

[CreateAssetMenu(menuName = "/Sounds SO", fileName = "Sounds SO")]
public class SoundSO : ScriptableObject
{
    public SoundList[] sounds;
}
