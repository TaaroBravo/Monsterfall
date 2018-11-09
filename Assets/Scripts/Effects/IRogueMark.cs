using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IRogueMark : IEffect
{
    Rogue _player;
    PlayerController target;
    int maxOfMarks;
    int countOfMarks;
    int damageMultiply;
    int damage;

    Action Disable;

    public IRogueMark(Rogue player, int damage, int maxOfMarks)
    {
        this.maxOfMarks = maxOfMarks;
        this.damage = damage;
        _player = player;
        Disable = DisableMark;
    }

    public void Effect(PlayerController player)
    {
        if (target == player)
        {
            countOfMarks++;
            if(countOfMarks >= 4)
            {
                countOfMarks = 4;
                damageMultiply = maxOfMarks;
            }
            SetDamage();
        }
        else
        {
            damageMultiply = 0;
            countOfMarks = 1;
            if (target)
                target.GetComponent<RogueSkillCall>().ResetFeedback();
            target = player;
            SetDamage();
        }
    }

    void DisableMark()
    {
        countOfMarks = 0;
        damageMultiply = 0;
    }

    public void DisableEffect(PlayerController player)
    {

    }

    void SetDamage()
    {
        target.SetDamage((damage + damageMultiply) * _player.buffedPower);
        target.GetComponent<RogueSkillCall>().PassState(countOfMarks - 1, _player.GetComponent<PlayerInput>().player_number, Disable);
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
