using UnityEngine;
using UnityEngine.Audio;

public class MixerTest : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        mixer.SetFloat("currentVolumeForEffects", -10.0f);
    }
}
