using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RailwayLevelManager : Triggerable
{
    private AudioSource AudioSource;
    public AudioClip mainTheme;
    public AudioClip runningTheme;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.loop = true;
        AudioSource.PlayOneShot(mainTheme);
    }


    public override void Trigger(TriggerAction action)
    {
        AudioSource.Stop();
        if (action == TriggerAction.Activate)
            AudioSource.PlayOneShot(mainTheme);
        else if (action == TriggerAction.Deactivate)
            AudioSource.PlayOneShot(runningTheme);
    }
}
