using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainParent : MonoBehaviour {

    public Hook hook;
    public Transform spawnPoint;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.up = -hook.transform.up;
        transform.position = spawnPoint.transform.position;
	}
}
