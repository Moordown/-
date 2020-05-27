using System;
using UnityEngine;

public class Item : Trigger
{
    public string ItemName;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<ItemSet>().Set.Add(ItemName);
        gameObject.SetActive(false);
    }
}