using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFeedbackControl : MonoBehaviour {

    public float timer;
    bool activate;
    public Material sarasa;
    public ParticleSystem ps;
	// Use this for initialization
	void Start () {
        sarasa.SetFloat("_Speed",0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && !activate)
        {
            activate = true;
            timer = 0;
            ps.Play();
        }
        if (activate)
        {
            timer += Time.deltaTime;
            //sarasa.SetFloat("_Speed", 0);
            if (timer > 0.7f) sarasa.SetFloat("_Speed", (timer -0.7f)*3f);
            else sarasa.SetFloat("_Speed", 0);
            if (timer >1) activate = false;
        }
	}
}
