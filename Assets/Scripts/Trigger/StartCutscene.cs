using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StartCutscene : MonoBehaviour
{
    public Triggerable Camera;
    public Triggerable Train;
    public GameObject Player;
    public AudioMixer mixer;

    public float SoundDropSpeed;
    
    private float EffectVolume;
    

    private void Awake()
    {
        mixer.GetFloat("currentVolumeForBackgroundEffects", out EffectVolume);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(RunCutscene(.5f));
    }
    
    IEnumerator RunCutscene(float time)
    {
        yield return new WaitForSeconds(time);
        
        // Player.GetComponent<CharacterMovement>().enabled = false;
        Camera.Trigger(TriggerAction.Activate);
        yield return new object();

        Player.transform.parent = Train.transform;
        Train.Trigger(TriggerAction.Activate);
        yield return new object();

        float value; 
        // sound effects mixer
        while (mixer.GetFloat("currentVolumeForBackgroundEffects", out value) && Math.Abs(value + 80) > 0.01)
        {
            value -= SoundDropSpeed * Time.deltaTime;
            value = Mathf.Clamp(value, -80f, 20);
            yield return mixer.SetFloat("currentVolumeForBackgroundEffects", value);
        }
    }
    
    
}
