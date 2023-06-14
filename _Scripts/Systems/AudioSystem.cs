using UnityEngine;
using System;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio Source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundsSource;

    public Sound[] sounds;

    void Start() {
        foreach (Sound s in sounds) {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    private void PlayGoblinKilled(BasicGoblin goblin) {
        PlaySound("enemyDeath", goblin.transform.position);
    }

    public void PlayMusic(string soundName) {
        Sound s = Array.Find(sounds, sound => sound.Name == soundName);
        if (s == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        musicSource.clip = s.Clip;
        musicSource.loop = s.Loop;
        musicSource.volume = s.Volume;

        musicSource.Play();
    }

    public void PlaySound(string soundName, Vector3 pos, float vol = 1) {
        Sound s = Array.Find(sounds, sound => sound.Name == soundName);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        soundsSource.transform.position = pos;
        soundsSource.clip = s.Source.clip;
        soundsSource.loop = s.Source.loop;
        soundsSource.volume = s.Source.volume;

        if (soundsSource.loop)
            soundsSource.Play();
        else
            soundsSource.PlayOneShot(soundsSource.clip, vol);
    }

    #region Events

    private void OnEnable() {
        BasicGoblin.GoblinKilled += PlayGoblinKilled;
        //BasicGoblin.GoblinDamaged;
        //BasicGoblin.GoblinSpawned;
    }

    private void OnDisable() {
        BasicGoblin.GoblinKilled -= PlayGoblinKilled;
    }

    #endregion
}