using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPart : MonoBehaviour {

    public const float CHAIN_LENGTH = 2f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Init(ChainPart c)
    {
        c.gameObject.SetActive(true);
    }

    public static void Finit(ChainPart c)
    {
        c.gameObject.SetActive(false);
    }
}
