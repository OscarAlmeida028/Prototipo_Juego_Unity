using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{get; private set;}
    private AudioSource audioSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Más de dos audio manager");
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RepodroducirSonido(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
