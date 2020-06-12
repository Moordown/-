
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
public class FightJudger : MonoBehaviour
{
    private Trigger trigger;
    private Dictionary<GameObject, bool> set;

    void Awake()
    {
        set = new Dictionary<GameObject, bool>();
        trigger = GetComponent<Trigger>();
    }

    public void OnDeath(GameObject obj)
    {
        if (set.TryGetValue(obj, out var value) && value) return;
        set[obj] = true;
        Debug.Log("Object dead");
        trigger.TriggerTargets();
        StartCoroutine(InactivateObject(obj));
    }

    IEnumerator InactivateObject(GameObject obj)
    {
        obj.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(140f/30);
        obj.SetActive(false);
        set[obj] = false;;
    }
}
