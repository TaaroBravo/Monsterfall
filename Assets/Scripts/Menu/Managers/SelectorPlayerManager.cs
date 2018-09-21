using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class SelectorPlayerManager : MonoBehaviour
{

    private static SelectorPlayerManager _instance;
    public static SelectorPlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    InputManager input = new InputManager();

    public List<PlayerAvatar> players = new List<PlayerAvatar>();
    List<PlayerAvatar> _playersInGame = new List<PlayerAvatar>();
    List<PlayerAvatar> _playersReady = new List<PlayerAvatar>();

    private void Awake()
    {
        _instance = this;
        Cursor.visible = false;
    }

    void Start()
    {
        List<PlayerInputMenu> playerInputs = new List<PlayerInputMenu>();
        foreach (var player in players)
        {
            player.OnSelectedCharacter += x => OnChosenCharacter(x);
            player.OnRejectedCharacter += x => OnRejectedCharacter(x);
            playerInputs.Add(player.GetComponent<PlayerInputMenu>());
        }
        input.Initializate(playerInputs);
    }

    void Update()
    {
        if (_playersInGame.Count() > 1 && _playersReady.Count() == _playersInGame.Count())
            SetUpInfo();
    }

    public void OnConnectedPlayer(PlayerInputMenu player)
    {
        if (_playersInGame.Count >= 4)
            return;

        PlayerAvatar newPlayer = input.OnConnectedPlayer(player);
        if (!newPlayer)
            return;
        _playersInGame.Add(newPlayer);
        newPlayer.inGame = true;
        newPlayer.GetComponent<Image>().color = players[_playersInGame.Count - 1].onColor;
    }

    public void OnDisconnectedPlayer(PlayerInputMenu player)
    {
        PlayerAvatar myAvatar = input.PickMyAvatar(player);
        if (!myAvatar)
            return;
        else if (myAvatar.ready)
            return;

        _playersInGame.Remove(myAvatar);
        myAvatar.inGame = false;
        myAvatar.GetComponent<Image>().color = myAvatar.offColor;
        myAvatar.GetComponent<PlayerInputMenu>().id = 0;
        input.DisconnecPlayer(myAvatar.GetComponent<PlayerInputMenu>());
    }

    void OnChosenCharacter(PlayerAvatar player)
    {
        _playersReady.Add(player);
    }

    void OnRejectedCharacter(PlayerAvatar player)
    {
        _playersReady.Remove(player);
    }

    void SetUpInfo()
    {
        GameObject.FindObjectOfType<PlayersInfoManager>().SetUpInfo(_playersReady);
    }
}
