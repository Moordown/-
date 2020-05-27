using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class CarriageOscillatingComponent : MonoBehaviour
{
    public float PulseStart = 1;
    
    public float SmallPulsePeriod = 2.5f;
    public float SmallPulseForce;

    public float BigPulsePeriod = 10f;
    public float BigPulseForce;

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    public AudioClip soundToPlay;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();

        InvokeRepeating(nameof(MakeSmallPulse), PulseStart, SmallPulsePeriod);
        InvokeRepeating(nameof(MakeBigPulse), PulseStart + BigPulsePeriod, BigPulsePeriod);
        InvokeRepeating(nameof(FireSound), PulseStart, SmallPulsePeriod);
    }

    void FireSound()
    {
        audioSource.PlayOneShot(soundToPlay);
    }

    void MakeSmallPulse()
    {
        var force = new Vector3(0, 1, 0) * SmallPulseForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    void MakeBigPulse()
    {
        var force = new Vector3(0, 1, 0) * SmallPulseForce;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }
}