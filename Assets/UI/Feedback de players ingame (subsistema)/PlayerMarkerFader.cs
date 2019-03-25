using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerFader : MonoBehaviour {

    SpriteRenderer myself;
    float fadetimer;
    PlayerController player;

    bool startToDiffuse;
	// Use this for initialization
	void Start () {
        myself = GetComponent<SpriteRenderer>();
        player = FindMyPlayer(transform.parent);
        StartCoroutine(WaitToDiffuse());
	}
	
	// Update is called once per frame
	void Update () {
        SetFlip();
        if(startToDiffuse)
        {
            fadetimer += Time.deltaTime;
            var c = new Color(1, 1, 1, 1 - fadetimer/ 5);
            myself.color = c;
        }
	}

    IEnumerator WaitToDiffuse()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            startToDiffuse = true;
            break;
        }
    }

    void SetFlip()
    {
        if (player.transform.localScale.z < 0)
            myself.flipX = true;
        else
            myself.flipX = false;
    }

    PlayerController FindMyPlayer(Transform trans)
    {
        if (trans.GetComponent<PlayerController>())
            return trans.GetComponent<PlayerController>();
        return FindMyPlayer(trans.parent);
    }
}
