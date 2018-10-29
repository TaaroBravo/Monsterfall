using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCristal : MonoBehaviour
{

    public Transform cristal;

    public PlayerController[] players;

    int layerMask;

    void Start()
    {
        SetLayers();
    }

    void Update()
    {
        CalculateCollisions();
    }

    void CalculateCollisions()
    {
        Vector3 v0 = (cristal.up + cristal.right).normalized;
        Vector3 v1 = (cristal.up + v0).normalized;
        Vector3 v2 = (cristal.up + v1).normalized;
        Vector3 v3 = (cristal.up + v2).normalized;
        Vector3 leftVector = cristal.up;
        Vector3 rightVector = v2;
        Vector3 middleVector = v3;


        foreach (var player in players)
        {
            //Vector3 playerDirDown = (player.transform.position - cristal.position).normalized;
            Debug.Log(player.name);
            Vector3 playerPosUp = new Vector3(player.transform.position.x, player.transform.position.y + player.GetComponent<Collider>().bounds.extents.y * 2, player.transform.position.z);
            Vector3 playerDirUp = (playerPosUp - cristal.position).normalized;

            Vector3 playerPosMiddle = new Vector3(player.transform.position.x, player.transform.position.y + player.GetComponent<Collider>().bounds.extents.y, player.transform.position.z);
            Vector3 playerDirMiddle = (playerPosMiddle - cristal.position).normalized;

            //if (Vector3.Angle(middleVector, playerDirDown) <= Vector3.Angle(leftVector, rightVector))
            //{
            //    RaycastHit info;
            //    if (Physics.Raycast(cristal.position, playerDirDown, out info, 1000, layerMask))
            //    {
            //        if (info.collider.gameObject.layer == 8)
            //            HitPlayer(player, playerDirDown);
            //    }
            //}
            if (Vector3.Angle(middleVector, playerDirUp) <= Vector3.Angle(leftVector, rightVector))
            {
                RaycastHit info;
                if (Physics.Raycast(cristal.position, playerDirUp, out info, 1000, layerMask))
                {
                    if (info.collider.gameObject.layer == 8)
                        HitPlayer(player, playerDirUp);
                }
            }
            else if (Vector3.Angle(middleVector, playerDirMiddle) <= Vector3.Angle(leftVector, rightVector))
            {
                RaycastHit info;
                if (Physics.Raycast(cristal.position, playerDirMiddle, out info, 1000, layerMask))
                {
                    if (info.collider.gameObject.layer == 8)
                        HitPlayer(player, playerDirMiddle);
                }
            }
        }
    }

    void SetLayers()
    {
        var layerMaskIgnore1 = 1 << 18;
        var layerMaskIgnore2 = 1 << 17;
        var layerMaskIgnore3 = 1 << 13;
        var layerMaskIgnore4 = 1 << 19;
        layerMask = layerMaskIgnore1 | layerMaskIgnore2 | layerMaskIgnore3 | layerMaskIgnore4;
        layerMask = ~layerMask;
    }

    void HitPlayer(PlayerController player, Vector3 dir)
    {
        player.HitByRay(dir);
        //Llamar a una funcion del PlayerController que stunee y haga todo lo que dice el doc.
    }
}
