using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRogueMark : IEffect
{
    Rogue _player;
    PlayerController target;
    int maxOfMarks;
    int countOfMarks;
    int damageMultiply;
    int damage;

    public IRogueMark(Rogue player, int damage, int maxOfMarks)
    {
        this.maxOfMarks = maxOfMarks;
        this.damage = damage;
        _player = player;
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

    public void DisableEffect(PlayerController player)
    {

    }

    void SetDamage()
    {
        target.SetDamage((damage + damageMultiply) * _player.buffedPower);
        target.GetComponent<RogueSkillCall>().PassState(countOfMarks - 1);
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
