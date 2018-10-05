using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFeedbackController : MonoBehaviour {

    public ParticleSystem p1;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayLanding()
    {
        p1.Play();
    }
}
