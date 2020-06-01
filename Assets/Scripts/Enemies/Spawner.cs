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
    private int _enemyDead;
    private readonly int _maxSpawned = 100;

    private readonly List<GameObject> _spawnObjects = new List<GameObject>();

    public void Start()
    {
        GameManager.RoundComplete += ResetRound;
        for (var i = 0; i < _maxSpawned; i++)
        {
            var instance = Instantiate(spawn, transform);
            _spawnObjects.Add(instance);
            instance.transform.parent = null;
            instance.transform.position = transform.position;
            instance.SetActive(false);
        }
        ResetRound();
    }

    private void ResetRound()
    {
        spawnsDead = false;
        _getAmount = Math.Min(startAmount, _maxSpawned);
        _enemyDead = 0;
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        if (_enemyDead == _getAmount)
            spawnsDead = true;
    }

    private IEnumerator SpawnObjects()
    {
        for (var i = 0; i < _getAmount; i++)
        {
            _spawnObjects[i].SetActive(true);
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    public override void Trigger(TriggerAction action)
    {
        if (action == TriggerAction.Activate)
            _enemyDead++;
    }
}