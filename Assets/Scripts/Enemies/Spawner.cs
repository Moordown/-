using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour, ILogicController
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
        TrainLogicController.RoundComplete += ResetRound;
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

    private IEnumerator spawnCoroutine;
    private void ResetRound()
    {
        spawnsDead = false;
        spawned = 0;
        _getAmount = Math.Min(_maxSpawned, startAmount);
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        spawnCoroutine = SpawnObjects();
        StartCoroutine(spawnCoroutine);
    }

    void Update()
    {
        if (spawned == _getAmount && _spawnObjects.Count(go => !go.activeSelf) == _maxSpawned)
            spawnsDead = true;
    }

    private int spawned;
    
    private IEnumerator SpawnObjects()
    {
        for (var i = spawned; i < _getAmount; i++)
        {
            spawned++;
            Debug.Log($"spawn object: {i+1} {Random.value}");
            _spawnObjects[i].SetActive(true);
            _spawnObjects[i].GetComponent<Collider>().enabled = true;
            _spawnObjects[i].GetComponent<EnemyHealth>().currentHealth =
                _spawnObjects[i].GetComponent<EnemyHealth>().Health;
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    public void StopLogic()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
        for (var i = 0; i < spawned; i++)
            _spawnObjects[i].GetComponent<NavMeshAgent>().enabled = false; 
    }

    public void StartLogic()
    {
        spawnCoroutine = SpawnObjects();
        StartCoroutine(spawnCoroutine);
        for (var i = 0; i < spawned; i++)
            _spawnObjects[i].GetComponent<NavMeshAgent>().enabled = true; 
    }
}