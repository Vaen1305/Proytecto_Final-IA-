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
        if (EstoyDescansando())  // Este m�todo debe estar en tu l�gica de comportamiento
        {
            health.Heal(healingPerSecond * Time.deltaTime);
        }
    }

    private bool EstoyDescansando()
    {
        // Verifica si el NPC est� en el nodo "Descanso" del BT
        return true; // Reemplaza con tu l�gica real
    }
}
