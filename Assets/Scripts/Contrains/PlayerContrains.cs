﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerContrains : MonoBehaviour {

    public event Action<Vector3, Vector3> OnTeleportPlayer = delegate { };

    Transform player;
    float myContrains_Z;

    public Transform rBottomPos;
    public Transform rTopPos;
    public Transform[] rDownPos;
    public Transform[] rUpPos;

    void Start ()
    {
        player = gameObject.transform;
        myContrains_Z = 0;
	}

    private void Update()
    {
        if (player.position.z != 0)
            player.position = new Vector3(player.position.x, player.position.y, myContrains_Z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
        {
            WarpController door = other.gameObject.GetComponent<WarpController>();
            door.WarpWithParent(transform);
            OnTeleportPlayer(door.zoneToTeleportHook.position, door.parentWarp.zoneToTeleportHook.position);
        }
    }
}
