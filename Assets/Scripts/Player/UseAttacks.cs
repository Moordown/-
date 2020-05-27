using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UseAttacks : MonoBehaviour
{
    
    public AudioClip shotSound;
    public AudioClip punchSound;
    
    public int ammoAmount = 10;
    public float meleeRepeatDelay = 0.25f;
    public GameObject projectile;
    public GameObject punchMesh;
    public Text ammoPanel;
    private bool punchActive;
    private bool _isammoPanelNotNull;
    private AudioSource source;
    
    private void Start()
    {
        _isammoPanelNotNull = ammoPanel != null;
        source = gameObject.GetComponent<AudioSource>();
        UpdateText();
        punchMesh.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (ammoAmount > 0)
            {
                ammoAmount--;
                UpdateText();
                var clone = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);
                source.PlayOneShot(shotSound);
                Destroy(clone, 2.0f);
            }
            else
            {
                if (!punchActive)
                {
                    punchActive = true;
                    source.PlayOneShot(punchSound);
                    StartCoroutine(MeleeAttack());
                }
            }
        }
    }

    void ApplyAmmo(int ammo)
    {
        ammoAmount += ammo;
        UpdateText();
    }

    void UpdateText()
    {
        if (_isammoPanelNotNull)
            ammoPanel.text = ammoAmount.ToString();
    }

    IEnumerator MeleeAttack()
    {
        punchMesh.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        punchMesh.SetActive(false);
        yield return new WaitForSeconds(meleeRepeatDelay);
        punchActive = false;
        yield return null;
    }
}