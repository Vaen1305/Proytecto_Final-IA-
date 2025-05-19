using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IACharacterControl : MonoBehaviour
{
    public NavMeshAgent agent { get; set; }
    public JabaliHealth health { get; set; }
    public IAEyeBase AIEye { get; set; }

    public virtual void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<JabaliHealth>();
        AIEye = GetComponent<IAEyeBase>();

    }
}
