using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
     
    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip gameWin;
    public AudioClip gameLose;
    public AudioClip explosion;
    public AudioClip scream;
    public AudioClip[] slide;

    public AudioClip button;

    

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayLose()
    {
        sfxSource.clip = gameLose;
        sfxSource.Play();
    }

    public void PlaySlide()
    {
        int index = Random.Range(0, slide.Length);
        sfxSource.PlayOneShot(slide[index]);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSound(AudioClip audio)
    {
        sfxSource.clip = audio;
        sfxSource.Stop();

    }
}
