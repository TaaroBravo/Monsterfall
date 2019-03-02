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
        AudioManager.Instance.CreateSound("IceDestruction");
        if(target)
        {
            target.frozen = false;
            target.canMove = true;
            target.canInteract = true;
            target.myAnim.SetBool("Stunned", false);
            if (((Yeti)player).frozenCharacter.Contains(target))
                ((Yeti)player).frozenCharacter.Remove(target);
        }

    }

    private void Update()
    {
        transform.position = target.transform.position;
        if (!target || target.isDead)
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        target.canMove = false;
        target.canInteract = false;
        target.FreezeReset();
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
