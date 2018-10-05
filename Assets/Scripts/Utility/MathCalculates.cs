using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCalculates : MonoBehaviour
{

    public static MathCalculates Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        var vVector1 = vPoint - vA;
        var vVector2 = (vB - vA).normalized;

        var d = Vector3.Distance(vA, vB);
        var t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        var vVector3 = vVector2 * t;

        var vClosestPoint = vA + vVector3;

        return vClosestPoint;
    }
}
