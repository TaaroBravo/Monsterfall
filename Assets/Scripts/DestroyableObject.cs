using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

    public float timeToDestroy;

	void Start ()
    {
        Destroy(gameObject, timeToDestroy);
	}

}
