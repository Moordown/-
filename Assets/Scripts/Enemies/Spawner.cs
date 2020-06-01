using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : Triggerable
{
    public GameObject spawn;
    public int startAmount = 1;
    public float delaySpawn = 1;
    public bool spawnsDead;

    private int _getAmount;
    private readonly int _maxSpawned = 100;

    private readonly List<GameObject> _spawnObjects = new List<GameObject>();

    public void Start()
    {
        GameManager.RoundComplete += ResetRound;
        _getAmount = 0;
        for (var i = 0; i < _maxSpawned; i++)
        {
            var instance = Instantiate(spawn, transform);
            _spawnObjects.Add(instance);

            instance.SetActive(false);
            instance.GetComponent<EnemyController>().ActiveOnAwake = true;
            instance.transform.parent = null;
            instance.transform.position = transform.position;
        }
        ResetRound();
    }

    private void ResetRound()
    {
        spawnsDead = false;
        _getAmount = Math.Min(_maxSpawned, startAmount);
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        if (_spawnObjects.Count(go => !go.activeSelf) == _maxSpawned)
            spawnsDead = true;
    }

    private IEnumerator SpawnObjects()
    {
        Debug.Log(_getAmount);
        for (var i = 0; i < _getAmount; i++)
        {
            Debug.Log("spawn object");
            _spawnObjects[i].SetActive(true);
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    public override void Trigger(TriggerAction action)
    {
        // if (action == TriggerAction.Activate)
        //     _enemyDead++;
    }
}