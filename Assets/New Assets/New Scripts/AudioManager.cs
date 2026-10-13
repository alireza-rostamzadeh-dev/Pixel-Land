using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSound;

    public AudioClip background;
    public AudioClip walking;
    public AudioClip death;


    void Start(){
        musicSource.clip = background;
        musicSource.Play();
    } 


    public void PlaySFX(AudioClip clip){

        SFXSound.PlayOneShot(clip);
    }

}
