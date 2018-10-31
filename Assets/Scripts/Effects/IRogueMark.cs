using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRogueMark : IEffect
{
    Rogue _player;
    PlayerController target;
    int maxOfMarks;
    int countOfMarks;
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
            countOfMarks += 2;
            if (countOfMarks >= maxOfMarks)
                countOfMarks = maxOfMarks;
            SetDamage();
        }
        else
        {
            countOfMarks = 0;
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
        target.SetDamage((damage + countOfMarks) * _player.buffedPower);
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
