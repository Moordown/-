using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartCutscene : SceneLoader
{
    public Triggerable Camera;
    public Triggerable Train;
    public GameObject Player;
    public AudioMixer mixer;

    public string NewSceneName;
    public float SoundDropSpeed;
    public float TransitionWaitTime = 5.0f;
    
    public GameObject FightInterface;

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Enter to startCutscene: {other.gameObject.name} {other.gameObject.CompareTag("Player")}");
        if (other.gameObject != Player) return;
        Run(NewSceneName);
    }

    public void Run(string name)
    {
        StartCoroutine(RunCutscene(.5f, name));
    }

    IEnumerator RunCutscene(float time, string name)
    {
        FightInterface.SetActive(false);
        yield return new WaitForSeconds(time);

        Camera.Trigger(TriggerAction.Activate);
        yield return new object();

        Player.transform.parent = Train.transform;
        Train.Trigger(TriggerAction.Activate);
        yield return new object();

        foreach (var key in Config.mixerValues.Keys)
            StartCoroutine(TurnOffSound(key));
        yield return new WaitForSeconds(TransitionWaitTime);
        StartLoading(name);
    }

    private IEnumerator TurnOffSound(string soundParameter)
    {
        float value;
        while (mixer.GetFloat(soundParameter, out value) && Math.Abs(value + 80) > 0.01)
        {
            value -= SoundDropSpeed * Time.deltaTime;
            value = Mathf.Clamp(value, -80f, 20);
            
            mixer.SetFloat(soundParameter, value);
            yield return value;
        }
    }
}
