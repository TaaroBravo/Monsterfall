using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerFader : MonoBehaviour {

    SpriteRenderer myself;
    float fadetimer;
	// Use this for initialization
	void Start () {
        myself = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        fadetimer += Time.deltaTime;
        var c = new Color(1, 1, 1, 1 - fadetimer / 10);
        myself.color = c;
	}
}
