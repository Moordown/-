﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : SceneLoader
{
    public GameObject panel;
    public GameObject mutant;
    public AudioClip playerDeadSound;

    public delegate void RestartRounds();
    public int MaxRounds;
    public static event RestartRounds RoundComplete;

    private int health;
    private int roundsSurvived;
    private int currentRound;
    private PlayerHealth playerHealth;
    private Text panelText;
    private AudioSource source;
    private bool isPlayerDead;

    public string MenuSceneName;
    public List<Spawner> spawners;

    void Start()
    {
        isPlayerDead = false;
        source = gameObject.GetComponent<AudioSource>();
        spawners = new List<Spawner>();
        
        Time.timeScale = 1;
        panel.SetActive(false);
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        panelText = panel.GetComponentInChildren<Text>();
        foreach (var o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            var go = (GameObject) o;
            if (go.activeInHierarchy && go.name.Contains("Spawner"))
            {
                var spawner = go.GetComponent<Spawner>();
                spawners.Add(spawner);
            }
        }
    }

    private bool isBossFight;

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = playerHealth.health;
        if (health > 0)
        {
            if (isBossFight) return;

            var deadCount = spawners.Count(s => s.spawnsDead);
            if (deadCount == spawners.Count && roundsSurvived == currentRound)
            {
                roundsSurvived++;
                panelText.text = $"Round {roundsSurvived} Completed!";
                foreach (var spawner in spawners)
                    spawner.amount = roundsSurvived + 1;
                panel.SetActive(true);
            }
            else if (roundsSurvived != currentRound && Input.GetButton("Fire2"))
            {
                currentRound = roundsSurvived;
                panel.SetActive(false);
                if (currentRound < MaxRounds)
                    RoundComplete?.Invoke();
                else
                {
                    isBossFight = true;
                    mutant.SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire2"))
            {
                StartLoading(MenuSceneName);
            }
            else
            {
                if (!isPlayerDead)
                    source.PlayOneShot(playerDeadSound);
                isPlayerDead = true;
                panel.SetActive(true);
                playerHealth.SetDangerEffect();
                panelText.text = $"Survived {roundsSurvived} Rounds";
            }
        }
    }

}