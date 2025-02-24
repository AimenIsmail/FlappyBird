using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSoucer;
    // [SerializeField] AudioSource flapSource;

    public AudioClip background;
    public AudioClip Eat;
    public AudioClip LaserShot;
    public AudioClip GameOver;

    ///////////Sound Button
    public Button musicToggleButton;  
    public Sprite musicOnSprite;      //  music ON
    public Sprite musicOffSprite;     // music OFF
    private Image buttonImage;        
    private bool isMusicOn = true;    


    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();

        // Get Button Image component
        buttonImage = musicToggleButton.GetComponent<Image>();

        // Set the button click listener
        musicToggleButton.onClick.AddListener(ToggleMusic);

        // Initialize button image
        UpdateButtonImage();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn; // Toggle state

        if (isMusicOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }

        // Update button image
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (isMusicOn)
        {
            buttonImage.sprite = musicOnSprite; // Show music on icon
        }
        else
        {
            buttonImage.sprite = musicOffSprite; // Show music off icon
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSoucer.PlayOneShot(clip);
    }
    
}
