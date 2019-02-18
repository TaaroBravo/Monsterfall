using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPart : MonoBehaviour {

    public const float CHAIN_LENGTH = 2f;
	
	void Update ()
    {
        if (!gameObject.activeSelf)
            Destroy(gameObject, 1f);
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
