using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            // Instancjujemy pasek ¿ycia nad przeciwnikiem
            Vector3 offset = new Vector3(0, 2f, 0); // wysokoœæ nad g³ow¹
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity);
            healthBarInstance.transform.SetParent(transform); // dziecko przeciwnika
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (healthBarInstance != null)
            Destroy(healthBarInstance);

        Destroy(gameObject);
    }
}
