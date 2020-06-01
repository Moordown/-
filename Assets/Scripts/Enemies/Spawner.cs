using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        spawned = 0;
        _getAmount = Math.Min(_maxSpawned, startAmount);
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        if (spawned == _getAmount && _spawnObjects.Count(go => !go.activeSelf) == _maxSpawned)
            spawnsDead = true;
    }

    private int spawned;
    
    private IEnumerator SpawnObjects()
    {
        for (var i = 0; i < _getAmount; i++)
        {
            spawned++;
            Debug.Log($"spawn object: {i+1} {Random.value}");
            // _spawnObjects[i].transform.position = transform.position;
            _spawnObjects[i].SetActive(true);
            _spawnObjects[i].GetComponent<Collider>().enabled = true;
            _spawnObjects[i].GetComponent<EnemyHealth>().currentHealth =
                _spawnObjects[i].GetComponent<EnemyHealth>().Health;
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    public override void Trigger(TriggerAction action)
    {
    }
}