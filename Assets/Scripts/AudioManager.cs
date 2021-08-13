using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip deathSound;
    public AudioClip drownSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.pitch = Random.Range(0.75f, 1f);
        audioSource.volume = 0.5f;
        //audioSource.Play();
        audioSource.PlayOneShot(sound);
    }
}
