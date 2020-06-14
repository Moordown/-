using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RailwayLevelManager : LevelManager
{
    public AudioClip playerDeadSound;
    public GameObject panel;
    public GameObject FightInterface;

    public CharacterMovement characterMovement;
    public MouseLook mouseLock;
    
    public PlayerHealth PlayerHealth;
    private AudioSource source;
    private Text panelText;
    void Start()
    {
        source = GetComponent<AudioSource>();
        panelText = panel.GetComponentInChildren<Text>();
    }

    private bool isPlayerDead;
    void Update()
    {
        if (PlayerHealth.health <= 0)
        {
            if (Input.GetButton("Fire2"))
            {
                FightInterface.SetActive(false);
                StartLoading(Config.MenuSceneName);
            }
            else
            {
                if (!isPlayerDead)
                    source.PlayOneShot(playerDeadSound);
                isPlayerDead = true;
                panel.SetActive(true);
                PlayerHealth.SetDangerEffect();

                panelText.text = $"Вы умерли из-за того что упали с большой высоты, лол (:";
                characterMovement.enabled = false;
                mouseLock.enabled = false;
            }
        }
    }
}
