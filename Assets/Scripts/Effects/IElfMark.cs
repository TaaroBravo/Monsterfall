using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IElfMark : IEffect
{
    Elf _player;
    List<PlayerController> targets;

    event Action<PlayerController> OnAddPlayers = delegate { };

    public IElfMark(Elf player, Action<PlayerController> OnAddcallBack)
    {
        _player = player;
        targets = new List<PlayerController>();
        OnAddPlayers += x => OnAddcallBack(x);
        OnAddPlayers += x => AddPlayer(x);
        _player.OnDisableEffect += x => DisableEffect(x);
        _player.OnCleanTargets += () => CleanTargets();
    }
    public void Effect(PlayerController player)
    {
        SetMark(player);
        AddPlayer(player);
    }

    public void CleanTargets()
    {
        foreach (var p in targets)
        {
            //Apagar particula de los enemigos
        }
        targets.Clear();
    }

    public void DisableEffect(PlayerController player)
    {
        //Apagar particula de enemigo:
        player.GetComponent<GeneralFeedback>().FinishElfMark();
        targets.Remove(player);
    }

    void AddPlayer(PlayerController player)
    {
        if (!targets.Contains(player))
            targets.Add(player);
        _player.AddTarget(player);
    }

    void SetMark(PlayerController player)
    {
        //Encender particula en el enemigo:
        int ID = _player.GetComponent<PlayerInput>().player_number;
        player.GetComponent<GeneralFeedback>().StartElfMark(ID);
    }

    public float GetDelayTimer()
    {
        return 10000;
    }

    public float GetMaxTimer()
    {
        return 1000000;
    }
}
