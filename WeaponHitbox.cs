using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdü, czy trafiono przeciwnika (np. po tagu)
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Miecz trafi≥: " + other.name);
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}