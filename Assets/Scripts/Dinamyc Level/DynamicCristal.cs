using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
        //CalculateCollisions();
    }

    public void CalculateCollisions(List<Tuple<float, float>> positions)
    {
        Vector3 v0 = (cristal.up + cristal.right).normalized;
        Vector3 v1 = (cristal.up + v0).normalized;
        Vector3 v2 = (cristal.up + v1).normalized;
        Vector3 v3 = (cristal.up + v2).normalized;
        Vector3 leftVector = cristal.up;
        Vector3 rightVector = v2;
        Vector3 middleVector = v3;

        List<PlayerController> playersHitted = new List<PlayerController>();
        foreach (var player in players)
        {
            if (!player)
                continue;
            Vector3 playerPosUp = new Vector3(player.transform.position.x, player.transform.position.y + player.GetComponent<Collider>().bounds.extents.y * 2, player.transform.position.z);
            Vector3 playerDirUp = (playerPosUp - cristal.position).normalized;

            //Vector3 playerPosMiddle = new Vector3(player.transform.position.x, player.transform.position.y + player.GetComponent<Collider>().bounds.extents.y, player.transform.position.z);
            //Vector3 playerDirMiddle = (playerPosMiddle - cristal.position).normalized;

            foreach (var pos in positions)
            {
                Vector3 finalPos = new Vector3(pos.Item1, pos.Item2);
                RaycastHit info;

                if (Physics.Raycast(cristal.position, (finalPos - cristal.position).normalized, out info, 1000, layerMask))
                {
                    if (info.collider.gameObject.layer == 8 || info.collider.gameObject.layer == LayerMask.GetMask("Hitbox") && !playersHitted.Contains(player))
                    {
                        if (TargetScript(info.transform) == player)
                        {
                            playersHitted.Add(player);
                            HitPlayer(player, playerDirUp);
                        }
                    }
                }
            }
        }
    }

    public PlayerController TargetScript(Transform t)
    {
        if (t.GetComponent<PlayerController>())
            return t.GetComponent<PlayerController>();
        if (t.parent == null)
            return null;
        return TargetScript(t.parent);
    }

    void SetLayers()
    {
        var layerMaskIgnore1 = 1 << 18;
        var layerMaskIgnore2 = 1 << 17;
        var layerMaskIgnore3 = 1 << 13;
        var layerMaskIgnore4 = 1 << 19;
        var layerMaskIgnore5 = 1 << 20;
        layerMask = layerMaskIgnore1 | layerMaskIgnore2 | layerMaskIgnore3 | layerMaskIgnore4 | layerMaskIgnore5;
        layerMask = ~layerMask;
    }

    void HitPlayer(PlayerController player, Vector3 dir)
    {
        player.HitByRay(dir);
    }
}