using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioClip levelCompleteSoundEffect;
    [SerializeField] private AudioSource levelCompleteAudioSource;
    [SerializeField] private AudioSource backgroundMusic;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public void PlayCompleteLevelSound()
    {
        levelCompleteAudioSource.PlayOneShot(levelCompleteSoundEffect);
    }

    public void StopMusic() => backgroundMusic.Stop();
    public void StartMusic() => backgroundMusic.Play();
}
