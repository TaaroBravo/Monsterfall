using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    public List<PlayerInputMenu> playerInputs = new List<PlayerInputMenu>();
    private List<PlayerInputMenu> playersInGame = new List<PlayerInputMenu>();

    public void Initializate(List<PlayerInputMenu> _inputs)
    {
        playerInputs = _inputs;
    }

    public PlayerAvatar OnConnectedPlayer(PlayerInputMenu player)
    {
        PlayerAvatar returnedPlayer = playerInputs[playersInGame.Count].GetComponent<PlayerAvatar>();
        if (CheckIfPlayerExists(player))
            return null;
        SwitchPlayersInputs(playerInputs[playersInGame.Count], player);
        playersInGame.Add(playerInputs[playersInGame.Count]);
        return returnedPlayer;
    }

    bool CheckIfPlayerExists(PlayerInputMenu player)
    {
        foreach (var input in playersInGame)
        {
            if (input.controller == player.controller && input.id == player.id)
                return true;
        }
            
        return false;
    }

    public PlayerAvatar PickMyAvatar(PlayerInputMenu player)
    {
        foreach (var input in playersInGame)
        {
            if (input.controller == player.controller && input.id == player.id)
                return input.GetComponent<PlayerAvatar>();
        }
        return null;
    }

    public void DisconnecPlayer(PlayerInputMenu player)
    {
        playersInGame.Remove(player);
    }

    void SwitchPlayersInputs(PlayerInputMenu playerA, PlayerInputMenu playerB)
    {
        PlayerInputMenu tempInput = playerA;

        playerA.controller = playerB.controller;
        playerA.id = playerB.id;

        playerB.controller = tempInput.controller;
        playerB.id = tempInput.id;
    }
}
