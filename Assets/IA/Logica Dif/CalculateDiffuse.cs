using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDiffuse : MonoBehaviour
{
    [Header("Conjuntos Difusos")]
    public FuzzyFunction cerca = new FuzzyFunction();
    public FuzzyFunction medio = new FuzzyFunction();
    public FuzzyFunction lejos = new FuzzyFunction();
    
    [Header("Variables de Control")]
    public float speedRotation;
    public float DistanceRay = 10f;
    public LayerMask mask = 1;
    
    [Header("Sensores")]
    public Transform pointSensor;
    
    [Header("Estado")]
    public bool Collider = false;
    public RaycastHit hit;
    
    void Start()
    {
        if (pointSensor == null)
            pointSensor = transform;
            
        // Inicializar conjuntos difusos si no están configurados
        InitializeFuzzySets();
    }
    
    void Update()
    {
        DetectObstacles();
    }
    
    void DetectObstacles()
    {
        if (Physics.Raycast(pointSensor.position, pointSensor.forward, out hit, DistanceRay, mask))
        {
            Collider = true;
            float distancia = hit.distance;
            
            // Aplicar lógica difusa
            CalculateAngle(distancia);
        }
        else
        {
            Collider = false;
            speedRotation = 0;
        }
    }
    
    void CalculateAngle(float distancia)
    {
        // Evaluar conjuntos difusos
        float cercaValue = cerca.Evaluate(distancia);
        float medioValue = medio.Evaluate(distancia);
        float lejosValue = lejos.Evaluate(distancia);
        
        // Defuzzificación usando método del centroide
        float numerador = (cercaValue * cerca.Singleton) + 
                         (medioValue * medio.Singleton) + 
                         (lejosValue * lejos.Singleton);
        
        float denominador = cercaValue + medioValue + lejosValue;
        
        if (denominador > 0)
            speedRotation = numerador / denominador;
        else
            speedRotation = 0;
            
        // Normalizar la salida
        speedRotation = Mathf.Clamp(speedRotation, 0f, 5f);
    }
    
    void InitializeFuzzySets()
    {
        // Configurar conjunto "cerca" (0 a 3 metros)
        if (cerca.Functioncurves == null || cerca.Functioncurves.keys.Length == 0)
        {
            cerca.Functioncurves = new AnimationCurve();
            cerca.Functioncurves.AddKey(0f, 1f);
            cerca.Functioncurves.AddKey(2f, 1f);
            cerca.Functioncurves.AddKey(4f, 0f);
            cerca.Singleton = 3f; // Velocidad alta de rotación
        }
        
        // Configurar conjunto "medio" (2 a 6 metros)
        if (medio.Functioncurves == null || medio.Functioncurves.keys.Length == 0)
        {
            medio.Functioncurves = new AnimationCurve();
            medio.Functioncurves.AddKey(2f, 0f);
            medio.Functioncurves.AddKey(4f, 1f);
            medio.Functioncurves.AddKey(6f, 0f);
            medio.Singleton = 1.5f; // Velocidad media de rotación
        }
        
        // Configurar conjunto "lejos" (5 metros en adelante)
        if (lejos.Functioncurves == null || lejos.Functioncurves.keys.Length == 0)
        {
            lejos.Functioncurves = new AnimationCurve();
            lejos.Functioncurves.AddKey(5f, 0f);
            lejos.Functioncurves.AddKey(7f, 1f);
            lejos.Functioncurves.AddKey(10f, 1f);
            lejos.Singleton = 0.5f; // Velocidad baja de rotación
        }
    }
    
    void OnDrawGizmos()
    {
        if (pointSensor != null)
        {
            Gizmos.color = Collider ? Color.red : Color.green;
            Gizmos.DrawRay(pointSensor.position, pointSensor.forward * DistanceRay);
            
            if (Collider)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.3f);
            }
        }
    }
}