using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource overworldMusic;
    public AudioSource atmosphereSound;
    public AudioSource menuMusic;
    public AudioClip moveSound;
    public AudioClip deathSound;
    public AudioClip drownSound;
    public AudioClip coinPickUp;
    public AudioClip gateClosed;
    public AudioClip victory;
    public AudioClip roosterCrow;
    public AudioClip GameOver;
    public AudioClip startWhistle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(AudioClip sound, float volume)
    {
        audioSource.clip = sound;
        audioSource.pitch = Random.Range(0.75f, 1f);
        audioSource.volume = volume;
        audioSource.PlayOneShot(sound);
    }

    public void StopOverworldAudio()
    {
        overworldMusic.Stop();
        atmosphereSound.Stop();
    }

    public void PlayOverworldAudio()
    {
        overworldMusic.Play();
        atmosphereSound.Play();
    }
}
