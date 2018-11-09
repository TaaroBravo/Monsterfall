using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFeedbackControl : MonoBehaviour
{

    public float timer;
    public bool activate;
    public Material sarasa;
    public ParticleSystem ps;
    // Use this for initialization
    void Start()
    {
        sarasa.SetFloat("_Speed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            timer += Time.deltaTime;
            //if (timer > 2) timer = 1;
            sarasa.SetFloat("_Speed", timer - 1f / 2f);
            //if (timer > 0.5f) sarasa.SetFloat("_Speed", (timer - 0.5f)); // asi estaba 
            //if (timer > 1f) sarasa.SetFloat("_Speed", 1);
            //else { sarasa.SetFloat("_Speed", timer); activate  }

            //else sarasa.SetFloat("_Speed", 0);
            //if (timer > 1.7f)
            //{
            //    activate = false;
            //    timer = 0;
            //    sarasa.SetFloat("_Speed", 1);
            //    //ps.Stop();
            //}
            //else sarasa.SetFloat("_Speed", 0);
        }
    }
}
