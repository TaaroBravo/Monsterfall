using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour {

	
	void Update ()
    {
        //foreach (var item in Physics.OverlapBox(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size * 0.9f))
        //{
        //    if (item.GetComponent<PlayerController>())
        //        item.transform.position = new Vector3(item.transform.position.x, GetComponent<BoxCollider>().bounds.max.y, item.transform.position.z);
        //}
    }
}
