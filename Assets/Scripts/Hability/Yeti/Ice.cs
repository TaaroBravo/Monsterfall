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
        target.myAnim.Play("GetHitDown");
        Destroy(gameObject, 3);
    }

    private void OnDestroy()
    {
        //Instanciar Feedback
        AudioManager.Instance.CreateSound("IceDestruction");
        if (((Yeti)player).frozenCharacter.Contains(target))
        {
            ((Yeti)player).frozenCharacter.Remove(target);
        }
    }

    private void Update()
    {
        transform.position = target.transform.position;
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
