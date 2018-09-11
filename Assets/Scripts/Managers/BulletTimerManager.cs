using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.PostProcessing;

public class BulletTimerManager : MonoBehaviour
{

    public List<PlayerController> allPlayers = new List<PlayerController>();

    private void Start()
    {
        GameManager.Instance.OnSpawnCharacters += x => SetPlayers(x);
    }

    void Update()
    {
        var markedPlayers = allPlayers.Where(x => x != null).Where(x => x.playerMarked);
        MarkStunnedPlayers(markedPlayers);

        foreach (var p in allPlayers.Where(x => x != null).Where(x => !x.playerMarked))
            p.transform.Find("MarkedGlow").gameObject.SetActive(false);
    }

    void SetPlayers(List<PlayerController> heroes)
    {
        allPlayers.Clear();
        allPlayers = heroes;
    }

    void MarkStunnedPlayers(IEnumerable<PlayerController> _players)
    {
        //Feedback del marcado.
        foreach (var p in _players)
            p.transform.Find("MarkedGlow").gameObject.SetActive(true);
    }
}
