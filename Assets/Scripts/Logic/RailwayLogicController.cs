using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RailwayLogicController : LogicController
{
    public AudioClip playerDeadSound;
    public GameObject notificationPanel;
    public GameObject fightInterface;
    public StartTimer timer;

    public CharacterMovement characterMovement;
    public MouseLook mouseLock;
    
    private PlayerHealth playerHealth;
    private GameObject player; 
    private AudioSource source;
    private Text panelText;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        notificationPanel.SetActive(false);
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        panelText = notificationPanel.GetComponentInChildren<Text>();
    }

    private bool isPlayerDead;
    void Update()
    {
        if (playerHealth.health <= 0)
        {
            if (Input.GetButton("Fire2"))
            {
                fightInterface.SetActive(false);
                StartLoading(Config.MenuSceneName);
            }
            else
            {
                if (!isPlayerDead)
                    source.PlayOneShot(playerDeadSound);
                isPlayerDead = true;
                notificationPanel.SetActive(true);
                playerHealth.SetDangerEffect();

                panelText.text = $"Упадешь и не поймают...";
                characterMovement.enabled = false;
                mouseLock.enabled = false;
            }
        }
    }

    public override void StartLogic() => SetEnabled(true);

    public override void StopLogic() => SetEnabled(false);

    void SetEnabled(bool value)
    {
        fightInterface.SetActive(value);
        timer.enabled = value;
        player.GetComponent<PlayerHealth>().enabled = value;
        player.GetComponent<CharacterMovement>().enabled = value;
        player.GetComponentInChildren<MouseLook>().enabled = value;
    }
}
