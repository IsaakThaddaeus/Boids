using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class FibonacciLattice
{

    public static List<Vector3> distributePoints(int n, float length)
    {
        List<Vector3> points = new List<Vector3>();

        float goldenRatio = (1 + Mathf.Pow(5, 0.5f)) / 2f;

        for (int i = 0; i < n; i++)
        {
            float theta = 2 * Mathf.PI * i / goldenRatio;
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / n);
            Vector3 point = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)) * length;
            points.Add(point);
        }

        return points;
    }


}
