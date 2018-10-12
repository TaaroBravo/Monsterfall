using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueFeedbackController : MonoBehaviour {
    public float YAxis;
    public float XAxis;
    public ParticleSystem p1;
    public ParticleSystem.MainModule p1main;
    public ParticleSystem.ShapeModule p1shape;
    public float caseN;
    public Material textshader1;
    public Material textshader2;
    public void Start()
    {
        p1main = p1.main;
        p1shape = p1.shape;
    }
    public void Update()
    {
        YAxis = GetComponent<PlayerInput>().MainVertical();
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        if (YAxis > 0.25f && XAxis < -0.25f)
        {
            p1main.startRotationZ = 2.4f;
            p1shape.rotation = new Vector3(0, 45, 0);
            p1main.startSpeed = 10;
        }
        else if (YAxis > 0.25f && XAxis > 0.25f)
        {
            p1main.startRotationZ = 0.8f;
            p1shape.rotation = new Vector3(0, 125, 0);
            p1main.startSpeed = 10;
        }
        else if (YAxis < -0.25f && XAxis < -0.25f)
        {
            p1main.startRotationZ = 0.8f;
            p1shape.rotation = new Vector3(0, 315, 0);
            p1main.startSpeed = 10;
        }
        else if (YAxis < -0.25f && XAxis > 0.25f)
        {
            p1main.startRotationZ = 2.4f;
            p1shape.rotation = new Vector3(0, 225, 0);
            p1main.startSpeed = 10;
        }
        else if (YAxis > 0.25f && XAxis < 0.25f || YAxis > 0.25f && XAxis > -0.25f)
        {
            p1main.startRotationZ = 1.6f;
            p1shape.rotation = new Vector3(0, 90, 0);
            p1main.startSpeed = 8;
        }
        else if (YAxis < -0.25f && XAxis < 0.25f || YAxis < -0.25f && XAxis > -0.25f)
        {
            p1main.startRotationZ = 1.6f;
            p1shape.rotation = new Vector3(0, 270, 0);
            p1main.startSpeed = 8;
        }
        else if (YAxis > -0.25f && XAxis > 0.25f || YAxis < 0.25f && XAxis > 0.25f)
        {
            p1main.startRotationZ = 0;
            p1shape.rotation = new Vector3(0, 180, 0);
            p1main.startSpeed = 8;
        }
        else if (YAxis > -0.25f && XAxis < -0.25f || YAxis < 0.25f && XAxis < -0.25f)
        {
            p1main.startRotationZ = 0;
            p1shape.rotation = new Vector3(0, 0, 0);
            p1main.startSpeed = 8;
        }
    }
    public void DashRogue()
    {
        p1.Play();
    }
    public void ChangeTextColor()
    {
        textshader1.SetColor("_Color", new Color(0, 0, 0));
        textshader2.SetColor("_Color", new Color(0, 0, 0));
    }
}
