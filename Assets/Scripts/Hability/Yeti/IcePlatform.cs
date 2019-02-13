using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class IcePlatform : MonoBehaviour
{
    //Implementar Feedback
    PlayerController player;

    private void Start()
    {
        //Ignorar colisiones con los bordes
        foreach (var item in GameObject.FindGameObjectsWithTag("Platform"))
        {
            foreach (Collider col in item.GetComponents<Collider>())
                Physics.IgnoreCollision(GetComponent<Collider>(), col, true);
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("Borders"))
        {
            foreach (Collider col in item.GetComponents<Collider>())
                Physics.IgnoreCollision(GetComponent<Collider>(), col, true);
        }
        foreach (var item in Physics.OverlapBox(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size))
        {
            if (item.GetComponent<PlayerController>() && item.GetComponent<PlayerController>() != player)
            {
                item.transform.position = new Vector3(item.transform.position.x, player.transform.position.y, item.transform.position.z);
            }
        }

        Destroy(gameObject, 4);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
    }

    public void SetPlayer(PlayerController p)
    {
        player = p;
    }

    private void OnDestroy()
    {
        //Instanciar feedback
    }
}