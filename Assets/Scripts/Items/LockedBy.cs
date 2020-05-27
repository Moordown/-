using System;
using UnityEngine;


public class LockedBy : Trigger
{
    public string lockerName;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var player = other.gameObject.GetComponent<ItemSet>();
        if (!player.Set.Contains(lockerName)) return;
        TriggerTargets();
        player.Set.Remove(lockerName);
    }
}