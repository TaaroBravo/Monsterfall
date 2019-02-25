using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IIceEffect : IEffect
{

    Yeti _player;
    PlayerController target;
    int maxOfMarks;
    int countOfMarks;

    Ice icePrefab;

    public IIceEffect(Yeti player, int maxOfMarks, Ice _icePrefab)
    {
        this.maxOfMarks = maxOfMarks;
        _player = player;
        icePrefab = _icePrefab;
    }

    public void Effect(PlayerController hitTarget)
    {
        if (target == hitTarget)
        {
            countOfMarks++;
            if (countOfMarks >= 6)
            {
                countOfMarks = 0;
                Freeze();
            }
            _player.CooldownMark(DisableEffect);
        }
        else
        {
            countOfMarks = 1;
            //if (target)
            //    target.GetComponent<RogueSkillCall>().ResetFeedback();
            target = hitTarget;
            _player.CooldownMark(DisableEffect);
        }
    }

    void Freeze()
    {
        if (_player.frozenCharacter.Contains(target))
            return;
        target.myAnim.Play("GetHit");
        target.SetStun(3f);
        target.ResetVelocity();
        target.SetLastOneWhoHittedMe(_player);
        var ice = GameObject.Instantiate(icePrefab);
        Physics.IgnoreCollision(ice.GetComponent<Collider>(), target.GetComponent<Collider>(), true);
        ice.transform.position = target.transform.position;
        _player.frozenCharacter.Add(target);
        ice.SetPlayer(_player, target);
    }


    public void DisableEffect(PlayerController player)
    {
        countOfMarks = 0;
        if (target)
        {
            //Sacar efecto de marca hielo
            //target.GetComponent<RogueSkillCall>().ResetFeedback();
            //target = null;
        }
    }

    public float GetDelayTimer()
    {
        return 1000;
    }

    public float GetMaxTimer()
    {
        return 1000;
    }
}
