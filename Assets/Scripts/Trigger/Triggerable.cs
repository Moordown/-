using UnityEngine;

public enum TriggerAction
{
    Activate,
    Deactivate,
    Toggle
}

public abstract class Triggerable : MonoBehaviour
{
    public abstract void Trigger(TriggerAction action);
}