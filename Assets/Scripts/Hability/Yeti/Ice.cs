using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{

    PlayerController player;
    PlayerController target;
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), target.GetComponent<Collider>(), true);
        Destroy(gameObject, 5);
    }

    private void OnDestroy()
    {
        //Instanciar Feedback
        if (((Yeti)player).frozenCharacter.Contains(target))
        {
            ((Yeti)player).frozenCharacter.Remove(target);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetPlayer(PlayerController p, PlayerController t)
    {
        player = p;
        target = t;
    }
}
