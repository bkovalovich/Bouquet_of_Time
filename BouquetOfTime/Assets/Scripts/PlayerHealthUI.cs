using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    public Image healthBar; // The UI Image representing the player's health
    private float currentHealth;
    private float maxHealth = 100f; // You can change this as needed
    public float tickDuration = 0.5f; // Time in seconds to complete the animation

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBarInstant();
    }

    public void TakeDamage(float damage)
    {
        float targetHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        StartCoroutine(AnimateHealthBar(currentHealth, targetHealth));
        currentHealth = targetHealth;
    }

    private IEnumerator AnimateHealthBar(float startHealth, float targetHealth)
    {
        float elapsedTime = 0f;

        while (elapsedTime < tickDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentFillAmount = Mathf.Lerp(startHealth, targetHealth, elapsedTime / tickDuration);
            healthBar.fillAmount = currentFillAmount / maxHealth;
            yield return null;
        }

        // Ensure the final value is set in case the loop doesn't hit the exact value
        healthBar.fillAmount = targetHealth / maxHealth;
    }

    private void UpdateHealthBarInstant()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
