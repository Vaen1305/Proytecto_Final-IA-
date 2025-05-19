using UnityEngine;

public class JabaliHea : MonoBehaviour
{
    [Header("Atributos de salud")]
    public float maxHealth = 100f;  // Salud m�xima
    public float currentHealth;     // Salud actual
    public bool isDead = false;     // Estado de muerte

    void Start()
    {
        currentHealth = maxHealth;
    }

    // M�todo para recibir da�o
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibi� da�o: " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para curarse
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log(gameObject.name + " se cur�: " + amount);
    }

    // Muerte del NPC
    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " ha muerto.");
        gameObject.SetActive(false);  // Desactiva el GameObject (puedes cambiar esto)
    }

    // Retorna si sigue vivo
    public bool IsAlive()
    {
        return !isDead;
    }
}
