using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCiervo : Health
{
    [Header("Ciervo Specific")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float tiredThreshold = 30f; // Cuando la stamina baja de este valor, el ciervo se cansa
    
    void Start()
    {
        _UnitGame = UnitGame.Ciervo;
        base.LoadComponent();
    }

    // Métodos específicos para el Ciervo
    public bool IsTired()
    {
        return stamina <= tiredThreshold;
    }

    public void RestoreStamina(float amount)
    {
        stamina = Mathf.Min(stamina + amount, maxStamina);
    }

    public void ConsumeStamina(float amount)
    {
        stamina = Mathf.Max(stamina - amount, 0f);
    }

    public float GetStaminaPercentage()
    {
        return stamina / maxStamina;
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        stamina = maxStamina;
    }

    private void Update()
    {
        // Regeneración pasiva de stamina cuando está descansando
        if (stamina < maxStamina)
        {
            stamina += Time.deltaTime * 5f; // Regenera 5 puntos por segundo
            stamina = Mathf.Min(stamina, maxStamina);
        }
    }
}