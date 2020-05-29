using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawners
{
    public GameObject go;
    public bool active;

    public Spawners(GameObject newGo, bool newBool)
    {
        go = newGo;
        active = newBool;
    }
}

[RequireComponent(typeof(Trigger))]
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public GameObject panel;
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
    private Trigger trigger;

    public List<Spawners> spawner = new List<Spawners>();

    void Start()
    {
        isPlayerDead = false;
        trigger = gameObject.GetComponent<Trigger>();
        source = gameObject.GetComponent<AudioSource>();
        
        Time.timeScale = 1;
        panel.SetActive(false);
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        panelText = panel.GetComponentInChildren<Text>();
        foreach (var o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            var go = (GameObject) o;
            if (go.name.Contains("Spawner"))
                spawner.Add(new Spawners(go, true));
        }
        Debug.LogWarning(spawner.Count());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        int total = 0;
        health = playerHealth.health;
        if (health > 0)
        {
            if (spawner.Count == 0) return;
            for (int i = spawner.Count - 1; i >= 0; i--)
            {
                if (spawner[i].go.GetComponent<Spawner>().spawnsDead)
                {
                    total++;
                }
            }

            if (total == spawner.Count && roundsSurvived == currentRound)
            {
                roundsSurvived++;
                panelText.text = $"Round {roundsSurvived} Completed!";
                foreach (var _spawner in spawner)
                    _spawner.go.GetComponent<Spawner>().amount = roundsSurvived + 1;
                panel.SetActive(true);
            }

            if (roundsSurvived != currentRound && Input.GetButton("Fire2"))
            {
                currentRound = roundsSurvived;
                panel.SetActive(false);
                if (currentRound == MaxRounds)
                    trigger.TriggerTargets();
                else
                    RoundComplete?.Invoke();
            }
        }
        else
        {
            if (Input.GetButton("Fire2"))
            {
                Scene current = SceneManager.GetActiveScene();
                SceneManager.LoadScene(current.name);
            }
            else
            {
                if (!isPlayerDead)
                    source.PlayOneShot(playerDeadSound);
                isPlayerDead = true;
                panel.SetActive(true);
                panelText.text = string.Format("Survived {0} Rounds", roundsSurvived);
                Time.timeScale = 0;
            }
        }
    }
}