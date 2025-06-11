using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FuzzyFunction 
{
    public AnimationCurve Functioncurves;
    public float F_y;
    public float Singleton;

    public FuzzyFunction()
    {
        // Constructor vacío
    }

    public FuzzyFunction(float singleton)
    {
        Singleton = singleton;
        Functioncurves = new AnimationCurve();
    }

    public float Evaluate(float x)
    {
        F_y = 0;
        if (Functioncurves != null && Functioncurves.keys.Length > 0)
        {
            if (x >= Functioncurves.keys[0].time)
                F_y = Mathf.Clamp01(Functioncurves.Evaluate(x));
        }
        return F_y;
    }

    public void SetCurvePoints(float[] times, float[] values)
    {
        if (Functioncurves == null)
            Functioncurves = new AnimationCurve();
        
        Functioncurves.keys = new Keyframe[0]; // Limpiar curva
        
        for (int i = 0; i < times.Length && i < values.Length; i++)
        {
            Functioncurves.AddKey(times[i], values[i]);
        }
    }
}