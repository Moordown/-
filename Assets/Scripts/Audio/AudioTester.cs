using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTester : MonoBehaviour
{
    public AudioClip soundToPlay;
    private AudioSource _source;

    // Update is called once per frame
    void Start()
    {
        _source = gameObject.GetComponent<AudioSource>();
        InvokeRepeating("FireSound", 1f, 5.0f);
    }

    void FireSound()
    {
        _source.PlayOneShot(soundToPlay);
    }
}
