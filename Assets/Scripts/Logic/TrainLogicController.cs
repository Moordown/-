using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TrainLogicController : LogicController
{
    public GameObject panel;
    public GameObject boss;
    
    public AudioClip playerDeadSound;

    public GameObject fightInterface;
    // public CharacterMovement characterMovement;
    // public MouseLook mouseLock;

    public StartDialogueByName intro;
    public StartDialogueByName end;

    public delegate void RestartRounds();
    public int MaxRounds;
    public static event RestartRounds RoundComplete;

    private int health;
    private int roundsSurvived;
    private int currentRound;
    private GameObject player;
    // private PlayerHealth playerHealth;
    private Text panelText;
    private AudioSource source;
    private bool isPlayerDead;

    public List<Spawner> spawners;

    void Awake()
    {
        isPlayerDead = false;
        source = gameObject.GetComponent<AudioSource>();

        spawners = new List<Spawner>();
        
        Time.timeScale = 1;
        panel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        panelText = panel.GetComponentInChildren<Text>();
        foreach (var go in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            if (go.activeInHierarchy)
            {
                Debug.Log(go.name);
                var spawner = go.GetComponent<Spawner>();
                spawners.Add(spawner);
            }
        }
        Debug.Log($"Spawner counts: {spawners.Count}");
    }

    void Start()
    {
        intro.Trigger(TriggerAction.Activate);
    }

    private bool isBossFight;

    void Update()
    {
        if (_isStopped) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = player.GetComponent<PlayerHealth>().health;
        if (health > 0)
        {
            if (isBossFight) return;

            var deadCount = spawners.Count(s => s.spawnsDead);
            if (deadCount == spawners.Count && roundsSurvived == currentRound)
            {
                roundsSurvived++;
                panelText.text = $"Round {roundsSurvived} Completed!";
                foreach (var spawner in spawners)
                    spawner.startAmount = roundsSurvived + 1;
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
                    boss.SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire2"))
            {
                Debug.Log("new scene");
                fightInterface.SetActive(false);
                StartLoading(Config.MenuSceneName);
            }
            else
            {
                if (!isPlayerDead)
                    source.PlayOneShot(playerDeadSound);
                isPlayerDead = true;
                panel.SetActive(true);
                player.GetComponent<PlayerHealth>().SetDangerEffect();
                panelText.text = $"Survived {roundsSurvived} Rounds";

                SetEnabled(false, false);
            }
        }
    }

    private bool _isStopped = false;

    public override void StartLogic()
    {
        _isStopped = false;
        SetEnabled(true);
        foreach (var spawner in spawners)
            spawner.StartLogic();
    }

    public override void StopLogic()
    {
        _isStopped = true;
        SetEnabled(false);
        foreach (var spawner in spawners)
            spawner.StopLogic();
    }

    void SetEnabled(bool value, bool andFightInterface = true)
    {
        if (andFightInterface)
            fightInterface.SetActive(value);
        player.GetComponent<Rigidbody>().isKinematic = !value;
        player.GetComponent<PlayerHealth>().enabled = value;
        player.GetComponent<CharacterMovement>().enabled = value;
        player.GetComponentInChildren<MouseLook>().enabled = value;
    }
}