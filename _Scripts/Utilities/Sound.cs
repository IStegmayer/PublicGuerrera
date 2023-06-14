using UnityEngine;

/// <summary>
/// Very simple class representation of sound objects
/// </summary>
[System.Serializable]
public class Sound
{
    public string Name;
    public bool Loop;

    public AudioClip Clip;
    [HideInInspector]
    public AudioSource Source;

    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float Pitch;
}