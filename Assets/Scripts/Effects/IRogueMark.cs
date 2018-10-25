using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRogueMark : IEffect
{
    PlayerController target;
    int maxOfMarks;
    int countOfMarks;
    int damage;

    public IRogueMark(int damage, int maxOfMarks)
    {
        this.maxOfMarks = maxOfMarks;
        this.damage = damage;
    }

    public void Effect(PlayerController player)
    {
        if(target == player)
        {
            countOfMarks++;
            SetDamage();
        }
        else
        {
            countOfMarks = 1;
            target = player;
            SetDamage();
        }
    }

    void SetDamage()
    {
        target.SetDamage(damage * countOfMarks);
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
