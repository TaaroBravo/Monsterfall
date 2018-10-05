using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFeedbackControl : MonoBehaviour {

    public float timer;
    public bool activate;
    public Material sarasa;
    public ParticleSystem ps;
	// Use this for initialization
	void Start () {
        sarasa.SetFloat("_Speed",0);
	}
	
	// Update is called once per frame
	void Update () {
        if (activate)
        {
            timer += Time.deltaTime;
            if (timer > 0.7f) sarasa.SetFloat("_Speed", (timer -0.7f)*3f);
            else sarasa.SetFloat("_Speed", 0);
            if (timer > 1)
            {
                activate = false;
                timer = 0;
            }
        }
	}
}
