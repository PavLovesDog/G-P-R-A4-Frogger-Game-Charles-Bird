using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // References to all audio played in-game
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

    // Function to call when another script needs to play a sound
    // takes in the audioclip and volume as float value
    public void PlayAudio(AudioClip sound, float volume)
    {
        audioSource.clip = sound;
        audioSource.pitch = Random.Range(0.75f, 1f);
        audioSource.volume = volume;
        audioSource.PlayOneShot(sound);
    }

    // function to Stop the game music and atmosphere sounds
    public void StopOverworldAudio()
    {
        overworldMusic.Stop();
        atmosphereSound.Stop();
    }

    // function to Play the game music and atmosphere sounds
    public void PlayOverworldAudio()
    {
        overworldMusic.Play();
        atmosphereSound.Play();
    }
}
