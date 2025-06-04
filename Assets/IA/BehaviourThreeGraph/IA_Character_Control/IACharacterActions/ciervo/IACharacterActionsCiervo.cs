using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACharacterActionsCiervo : IACharacterActions
{
    private HealthCiervo _healthCiervo;
    private IACharacterVehiculoCiervo ciervoVehicle;

    public override void LoadComponent()
    {
        base.LoadComponent();
        _healthCiervo = GetComponent<HealthCiervo>();
        ciervoVehicle = GetComponent<IACharacterVehiculoCiervo>();
    }

    // Métodos específicos para acciones del Ciervo
    public bool IsTired()
    {
        if (_healthCiervo != null)
        {
            return _healthCiervo.IsTired();
        }
        return false;
    }

    public void Rest()
    {
        if (ciervoVehicle != null)
        {
            ciervoVehicle.Rest();
        }
    }

    public void RunAway()
    {
        if (ciervoVehicle != null && ciervoVehicle.AIEye != null)
        {
            // Si hay un enemigo visible, huir de él
            if (ciervoVehicle.AIEye.ViewEnemy != null)
            {
                ciervoVehicle.RunAwayFrom(ciervoVehicle.AIEye.ViewEnemy.transform.position);
            }
            // Si es un IAEyeCiervo, usar la posición de amenaza más cercana (reflexión)
            else
            {
                var eyeComp = ciervoVehicle.AIEye as Component;
                if (eyeComp != null && eyeComp.GetType().Name == "IAEyeCiervo")
                {
                    var method = eyeComp.GetType().GetMethod("GetClosestThreatPosition");
                    if (method != null)
                    {
                        Vector3 threatPos = (Vector3)method.Invoke(eyeComp, null);
                        if (threatPos != Vector3.zero)
                        {
                            ciervoVehicle.RunAwayFrom(threatPos);
                        }
                    }
                }
            }
        }
    }

    public void Avoid()
    {
        if (ciervoVehicle != null && ciervoVehicle.AIEye != null)
        {
            var eyeComp = ciervoVehicle.AIEye as Component;
            if (eyeComp != null && eyeComp.GetType().Name == "IAEyeCiervo")
            {
                var viewedJabaliProp = eyeComp.GetType().GetProperty("ViewedJabali");
                var viewedJabali = viewedJabaliProp != null ? viewedJabaliProp.GetValue(eyeComp, null) as Health : null;
                if (viewedJabali != null)
                {
                    ciervoVehicle.AvoidPosition(viewedJabali.transform.position);
                }
                else if (ciervoVehicle.AIEye.ViewEnemy != null)
                {
                    ciervoVehicle.AvoidPosition(ciervoVehicle.AIEye.ViewEnemy.transform.position);
                }
            }
        }
    }
}