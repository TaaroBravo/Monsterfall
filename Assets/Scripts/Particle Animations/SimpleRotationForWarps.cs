using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotationForWarps : MonoBehaviour
{

    public bool imadoor;
    public bool streechdir;
    public float strechtimer;
    public float testimer;
    public float testtimemulti;

    void Start()
    {
        streechdir = true;
    }

    void Update()
    {
        strechtimer += Time.deltaTime;
        if (!imadoor) transform.Rotate(0, 0.5f, 0);
        else
        {
            transform.Rotate(1, 0, 0);
        }
    }
}
