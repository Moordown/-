using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartCutscene : SceneLoader
{
    public Triggerable Camera;
    public Triggerable Train;
    public GameObject Player;
    public AudioMixer mixer;

    // public Animator sceneTransitionAnimator;
    public string MenuSceneName;
    public float SoundDropSpeed;

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(RunCutscene(.5f));
    }
    
    IEnumerator RunCutscene(float time)
    {
        yield return new WaitForSeconds(time);
        
        Camera.Trigger(TriggerAction.Activate);
        yield return new object();

        Player.transform.parent = Train.transform;
        Train.Trigger(TriggerAction.Activate);
        yield return new object();

        StartCoroutine(TurnOffSound());
        yield return new WaitForSeconds(5.0f);
        StartLoading(MenuSceneName);
        // StartCoroutine(LoadNewScene(MenuSceneName));
    }
    
    // IEnumerator LoadNewScene(string sceneName)
    // {
    //     sceneTransitionAnimator.SetTrigger("end");
    //     yield return new WaitForSeconds(1.5f);
    //     SceneManager.LoadScene(sceneName);
    // }

    private IEnumerator TurnOffSound()
    {
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
