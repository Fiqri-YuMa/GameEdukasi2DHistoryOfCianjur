using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthSlider; // Masukkan Slider dari Unity Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Mencegah darah di bawah 0 atau di atas max

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth / maxHealth; // Menghitung persentase (0 sampai 1)
    }

    void Die()
    {
        Destroy(gameObject); // Hancurkan musuh saat darah habis
    }
}
