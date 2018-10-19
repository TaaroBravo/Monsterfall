using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }

    public GameObject[] pointBars = new GameObject[0];
    public Image roundNumber;
    public Sprite[] roundCounters = new Sprite[5];
    public int round;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        roundNumber.sprite = roundCounters[round];
    }

    //void Update () {
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        var pl = new List<PlayerInfo>();
    //        pl.Add(new PlayerRoundStats(3, PointsBar.Characters.Knight,2,4));
    //        pl.Add(new PlayerRoundStats(1, PointsBar.Characters.Pirate,4,2));
    //        pl.Add(new PlayerRoundStats(0, PointsBar.Characters.Rogue,3,1));
    //        pl.Add(new PlayerRoundStats(2, PointsBar.Characters.Berserker, 2, 1));
    //        LoadBars(pl);
    //    }
    //}

    public void LoadBars(List<PlayerInfo> playerInfo, Action callBack)
    {
        playerInfo.OrderBy(x => x.newKills + x.previousKills);
        for (int i = 0; i < playerInfo.Count; i++)
        {
            pointBars[i].SetActive(true);
            var pb = pointBars[i].GetComponent<PointsBar>();
            pb.SetCharacterSpriteAndColor(playerInfo[i]);
            pb.sm.ActivateSkulls(playerInfo[i].previousKills);
            pb.sm.NormalizeSkulls();
            pb.sm.ActivateSkulls(playerInfo[i].newKills);
            playerInfo[i].previousKills += playerInfo[i].newKills;
            playerInfo[i].newKills = 0;

            playerInfo[i].round++;
        }
        callBack();
    }

    public void SetRound(int _round)
    {
        round = _round;
    }
}
