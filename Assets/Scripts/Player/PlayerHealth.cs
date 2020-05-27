using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    public Image damageFX;

    public int health = 100;
    private float maxAlpha = 0.7f;
    private bool isActive;

    public AudioClip audioClip;
    private AudioSource audioSource;
    private bool _isaudioSourceNotNull;
    private bool _isaudioClipNotNull;

    private void Start()
    {
        UpdateText();
        audioSource = GetComponent<AudioSource>();
        _isaudioClipNotNull = audioClip != null;
        _isaudioSourceNotNull = audioSource != null;
    }

    void ApplyDamage(int damage)
    {
        health -= damage;
        UpdateText();
        if (!isActive && damageFX != null)
            StartCoroutine(SetEffect());
    }

    void ApplyHeal(int heal)
    {
        health += heal;
        UpdateText();
    }

    void UpdateText()
    {
        health = Mathf.Clamp(health, 0, 100);
        if (healthText == null) return;
        healthText.text = health.ToString();
    }

    private IEnumerator SetEffect()
    {
        isActive = true;
        // float alpha = damageFX.color.a;
        Color color = damageFX.color;
        damageFX.color = new Color(color.r, color.g, color.b, maxAlpha);
        if (_isaudioSourceNotNull && _isaudioClipNotNull)
            audioSource.PlayOneShot(audioClip);

        yield return new WaitForSeconds(0.2f);
        damageFX.color = new Color(color.r, color.g, color.b, 0);
        yield return new WaitForSeconds(0.4f);
        isActive = false;
        yield return null;
    }
}