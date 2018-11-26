using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackElf : MonoBehaviour
{

    public Transform spawnPointDash;
    public Transform spawnPointHability;
    public ParticleSystem shootFire;
    public ParticleSystem.ShapeModule DashShootShape;
    public ParticleSystem shootDashFire;
    public ParticleSystem DashTrail;
    public ParticleSystem.EmissionModule DashTrailEmission;
    public ParticleSystem DashFinish;
    public float XAxis;
    public bool iminverted;
    public bool teleporting;

    public void Start()
    {
        DashShootShape = shootDashFire.shape;
        DashTrailEmission = DashTrail.emission;
    }
    private void Update()
    {
        //DebugKeys();
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        DashTrailEmission.rateOverDistance = teleporting ? 10 : 0; // trail del teleport
        if (!iminverted) // esto quizas lo tenga k cambiar
        {
            if (XAxis > 0)
            {
                DashShootShape.rotation = new Vector3(0, 0, 0);
            }
            else if (XAxis < 0)
            {
                DashShootShape.rotation = new Vector3(180, 0, 0);
            }
        }
        else
        {
            if (XAxis > 0)
            {
                DashShootShape.rotation = new Vector3(180, 0, 0);
            }
            else if (XAxis < 0)
            {
                DashShootShape.rotation = new Vector3(0, 0, 0);
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
