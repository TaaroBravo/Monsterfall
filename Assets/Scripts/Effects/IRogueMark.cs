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
            _player.CooldownMark(DisableEffect);
        }
        else
        {
            damageMultiply = 0;
            countOfMarks = 1;
            if (target)
                target.GetComponent<RogueSkillCall>().ResetFeedback();
            target = player;
            SetDamage();
            _player.CooldownMark(DisableEffect);
        }
    }

    void DisableMark()
    {
        countOfMarks = 0;
        damageMultiply = 1;
        target = null;
    }

    public void DisableEffect(PlayerController player)
    {
        countOfMarks = 0;
        damageMultiply = 1;
        target.GetComponent<RogueSkillCall>().ResetFeedback();
        target = null;
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
