using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSoucer;
    // [SerializeField] AudioSource flapSource;

    public AudioClip background;
    public AudioClip Eat;
    public AudioClip Flap;
    public AudioClip GameOver;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true; 
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSoucer.PlayOneShot(clip);
    }
    // public void PlayFlapSound()
    // {
    //     flapSource.PlayOneShot(Flap); 
    // }
}
