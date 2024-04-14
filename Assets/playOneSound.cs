using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playOneSound : MonoBehaviour
{
    public AudioClip currentClip;
    public AudioSource audioSource;

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.5f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = currentClip;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, ((volumeMax - volumeMin) / 4) + volumeMin);
        audioSource.PlayOneShot(currentClip);
    }


}
