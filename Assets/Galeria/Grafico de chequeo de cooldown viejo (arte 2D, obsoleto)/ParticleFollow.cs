using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour {

    public string pathname;
    public float speed;
	// Use this for initialization
	void Start () {
        //iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathname), "easeType", iTween.EaseType.easeInOutSine, "time", speed));
	}
	// Update is called once per frame
	void Update () {
		
	}
}
