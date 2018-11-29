using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileTrail : MonoBehaviour {
    Vector3 startpos;
    Vector3 endpos;
    LineRenderer toModify;
    public Color lineColor;
    
    public void SetPositions(Vector3 pos1, Vector3 pos2)
    {
        startpos = pos1;
        endpos = pos2;
    }

    public void SetColor(Color playerColor)
    {
        toModify.material.SetColor("_PlayerColor", lineColor);
    }

    private void Start()
    {
        toModify = GetComponent<LineRenderer>();
        toModify.SetPosition(0, startpos);
        toModify.SetPosition(1, endpos);
    }

    private void Update()
    {
        toModify.SetPosition(0, startpos);
        toModify.SetPosition(1, endpos);
    }

}
