using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateFeedbackController : MonoBehaviour {

    public ParticleSystem slashForward;
    public ParticleSystem slashDown;
    public ParticleSystem slashUp;


    public ParticleSystem.MainModule forwardMain;
    public ParticleSystem.MainModule downMain;
    public ParticleSystem.MainModule upMain;

    public ParticleSystem.ShapeModule slashForwardShape;
    public ParticleSystem.ShapeModule slashDownShape;
    public ParticleSystem.ShapeModule slashUpShape;



    public float XAxis;
    public bool iminverted;
    

    private void Start()
    {
        XAxis = iminverted == true ? -0.0001f : 0.0001f;

        forwardMain  = slashForward.main;
        downMain = slashDown.main;
        upMain = slashUp.main;

        slashForwardShape = slashForward.shape;
        slashDownShape = slashDown.shape;
        slashUpShape = slashUp.shape;
        
    }

    private void Update()
    {
        if(GetComponent<PlayerInput>().MainHorizontal() != 0) XAxis = GetComponent<PlayerInput>().MainHorizontal();
        if (!iminverted)
        {
            if (XAxis > 0)
            {
                forwardMain.startRotationY = 3.4f;
                downMain.startRotationY = 3.4f;
                upMain.startRotationY = 3.4f;
                slashForwardShape.rotation = new Vector3(0, 0, 0);
                slashDownShape.rotation = new Vector3(0, 0, 0);
                slashUpShape.rotation = new Vector3(0, 0, 0);
            }
            else if (XAxis < 0)
            {

                forwardMain.startRotationY = 0;
                downMain.startRotationY = 0;
                upMain.startRotationY = 0;
                slashForwardShape.rotation = new Vector3(0, 0, 0);
                slashDownShape.rotation = new Vector3(0, 0, 0);
                slashUpShape.rotation = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (XAxis > 0)
            {
                forwardMain.startRotationY = 3.4f;
                slashForwardShape.rotation = new Vector3(0, 0, 0);

                downMain.startRotationY = 3.4f;
                slashDownShape.rotation = new Vector3(0, 180, 0);

                upMain.startRotationY = 3.4f;
                slashUpShape.rotation = new Vector3(0, 180, 0);
            }
            else if (XAxis < 0)
            {
                forwardMain.startRotationY = 0;
                slashForwardShape.rotation = new Vector3(0, 180, 0);

                downMain.startRotationY = 0;
                slashDownShape.rotation = new Vector3(0, 180, 0);

                upMain.startRotationY = 0;
                slashUpShape.rotation = new Vector3(0, 180, 0);
            }
        }
    }

    public void DisplayForward() { slashForward.Play(); }
    public void DisplayDown() { slashDown.Play(); }
    public void DisplayUp() { slashUp.Play(); }
}

