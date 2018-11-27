using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackElf : MonoBehaviour
{

    public Transform spawnPointDash;
    public Transform spawnPointHability;
    public ParticleSystem shootFire;
    public ParticleSystem.ShapeModule ShootShape;
    public ParticleSystem shootFire2;
    public ParticleSystem.ShapeModule ShootShape2;
    public ParticleSystem shootFire3;
    public ParticleSystem.ShapeModule ShootShape3;
    public ParticleSystem shootDashFire;
    public ParticleSystem.ShapeModule DashShootShape;
    public ParticleSystem shootDashFire2;
    public ParticleSystem.ShapeModule DashShootShape2;
    public ParticleSystem DashTrail;
    public ParticleSystem.EmissionModule DashTrailEmission;
    public ParticleSystem DashFinish;
    public float XAxis;
    public float YAxis;
    public bool iminverted;
    public bool teleporting;

    public void Start()
    {
        DashShootShape = shootDashFire.shape;
        DashShootShape2 = shootDashFire2.shape;
        ShootShape = shootFire.shape;
        ShootShape2 = shootFire2.shape;
        ShootShape3 = shootFire3.shape;
        DashTrailEmission = DashTrail.emission;
    }
    private void Update()
    {
        //DebugKeys();
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        YAxis = GetComponent<PlayerInput>().MainVertical();
        DashTrailEmission.rateOverDistance = teleporting ? 10 : 0; // trail del teleport
        if (!iminverted) // esto quizas lo tenga k cambiar
        {
            if (XAxis > 0)
            {
                ShootShape.rotation = new Vector3(0, 180, 0);
                ShootShape2.rotation = new Vector3(0, 180, 0);
                ShootShape3.rotation = new Vector3(0, 180, 0);
            }
            else if (XAxis < 0)
            {
                ShootShape.rotation = new Vector3(0, 0, 0);
                ShootShape2.rotation = new Vector3(0, 0, 0);
                ShootShape3.rotation = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (XAxis > 0)
            {
                ShootShape.rotation = new Vector3(0, 0, 0);
                ShootShape2.rotation = new Vector3(0, 0, 0);
                ShootShape3.rotation = new Vector3(0, 0, 0);
            }
            else if (XAxis < 0)
            {
                ShootShape.rotation = new Vector3(0, 180, 0);
                ShootShape2.rotation = new Vector3(0, 180, 0);
                ShootShape3.rotation = new Vector3(0, 180, 0);
            }
        }
        if (!iminverted)
        {
            if (YAxis > 0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 315, 0);
                DashShootShape2.rotation = new Vector3(0, 315 + 90, 0);
            }
            else if (YAxis > 0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 225, 0);
                DashShootShape2.rotation = new Vector3(0, 225 - 90, 0);
            }
            else if (YAxis < -0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 45, 0);
                DashShootShape2.rotation = new Vector3(0, 45 - 90, 0);
            }
            else if (YAxis < -0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 135, 0);
                DashShootShape2.rotation = new Vector3(0, 135 + 90, 0);
            }
            else if (YAxis > 0.25f && XAxis < 0.25f || YAxis > 0.25f && XAxis > -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 270, 0);
                DashShootShape2.rotation = new Vector3(0, 90, 0);
            }
            else if (YAxis < -0.25f && XAxis < 0.25f || YAxis < -0.25f && XAxis > -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 90, 0);
                DashShootShape2.rotation = new Vector3(0, 270, 0);
            }
            else if (YAxis > -0.25f && XAxis > 0.25f || YAxis < 0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 180, 0);
                DashShootShape2.rotation = new Vector3(0, 180, 0);
            }
            else if (YAxis > -0.25f && XAxis < -0.25f || YAxis < 0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 0, 0);
                DashShootShape2.rotation = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (YAxis > 0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 225, 0);
                DashShootShape2.rotation = new Vector3(0, 225 - 90, 0);
            }
            else if (YAxis > 0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 315, 0);
                DashShootShape2.rotation = new Vector3(0, 315 + 90, 0);
            }
            else if (YAxis < -0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 125, 0);
                DashShootShape2.rotation = new Vector3(0, 125 + 90, 0);
            }
            else if (YAxis < -0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 45, 0);
                DashShootShape2.rotation = new Vector3(0, 45 - 90, 0);
            }
            else if (YAxis > 0.25f && XAxis < 0.25f || YAxis > 0.25f && XAxis > -0.25f) // funca
            {
                DashShootShape.rotation = new Vector3(0, 270, 0);
                DashShootShape2.rotation = new Vector3(0, 90, 0);
            }
            else if (YAxis < -0.25f && XAxis < 0.25f || YAxis < -0.25f && XAxis > -0.25f) // funca
            {
                DashShootShape.rotation = new Vector3(0, 90, 0);
                DashShootShape2.rotation = new Vector3(0, 270, 0);
            }
            else if (YAxis > -0.25f && XAxis > 0.25f || YAxis < 0.25f && XAxis > 0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 0, 0);
                DashShootShape2.rotation = new Vector3(0, 0, 0);
            }
            else if (YAxis > -0.25f && XAxis < -0.25f || YAxis < 0.25f && XAxis < -0.25f)
            {
                DashShootShape.rotation = new Vector3(0, 180, 0);
                DashShootShape2.rotation = new Vector3(0, 180, 0);
            }
        }
    }
    //private void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.M)) StartTeleportFeedback();
    //    if (Input.GetKeyDown(KeyCode.P)) FinishTeleportFeedback();
    //}
    public void StartTeleportFeedback()
    {
        teleporting = true;
    }
    public void FinishTeleportFeedback()
    {
        DashFinish.Play();
        teleporting = false;
    }
}
