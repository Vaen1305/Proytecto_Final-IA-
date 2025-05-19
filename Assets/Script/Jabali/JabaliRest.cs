using UnityEngine;

public class JabaliRest : MonoBehaviour
{
    public float healingPerSecond = 5f;
    private JabaliHealth health;

    void Start()
    {
        health = GetComponent<JabaliHealth>();
    }

    void Update()
    {
        if (EstoyDescansando())  // Este método debe estar en tu lógica de comportamiento
        {
            health.Heal(healingPerSecond * Time.deltaTime);
        }
    }

    private bool EstoyDescansando()
    {
        // Verifica si el NPC está en el nodo "Descanso" del BT
        return true; // Reemplaza con tu lógica real
    }
}
