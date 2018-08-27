using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.PostProcessing;

public class BulletTimerManager : MonoBehaviour
{

    public List<PlayerController> allPlayers;
    //public float distance;

    //public PostProcessingBehaviour myCamProf;
    //public PostProcessingProfile slowTimeSO;

    //private bool slowTime;
    //private float timer;

    //private ChromaticAberrationModel.Settings chromaticSettings;

    private void Start()
    {
        //myCamProf = Camera.main.GetComponent<PostProcessingBehaviour>();
        //chromaticSettings = slowTimeSO.chromaticAberration.settings;
    }

    void Update()
    {
        var markedPlayers = allPlayers.Where(x => x != null).Where(x => x.playerMarked);
        MarkStunnedPlayers(markedPlayers);

        foreach (var p in allPlayers.Where(x => x != null).Where(x => !x.playerMarked))
            p.transform.Find("MarkedGlow").gameObject.SetActive(false);
    }

    void MarkStunnedPlayers(IEnumerable<PlayerController> _players)
    {
        //Feedback del marcado.
        foreach (var p in _players)
            p.transform.Find("MarkedGlow").gameObject.SetActive(true);
    }
}
