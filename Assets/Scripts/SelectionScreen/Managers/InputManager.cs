using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{

    public List<PlayerInputMenu> playerInputs = new List<PlayerInputMenu>();
    private List<PlayerInputMenu> playersInGame = new List<PlayerInputMenu>();

    public void Initializate(List<PlayerInputMenu> _inputs)
    {
        playerInputs = _inputs;
    }

    public PlayerAvatar OnConnectedPlayer(PlayerInputMenu player, int ID)
    {
        PlayerAvatar returnedPlayer = playerInputs[ID - 1].GetComponent<PlayerAvatar>();
        if (CheckIfPlayerExists(player))
            return null;
        if (CheckIfInputExists(ID))
            returnedPlayer = SwitchPlayersInputs(returnedPlayer, player);
        else
            SetPlayerInput(returnedPlayer, player);
        playersInGame.Add(returnedPlayer.GetComponent<PlayerInputMenu>());
        return returnedPlayer;
    }

    bool CheckIfInputExists(int ID)
    {
        foreach (var input in playersInGame)
        {
            if (input.id == ID)
                return true;
            if (input.GetComponent<PlayerAvatar>().player_number == ID - 1)
                return true;
        }
        return false;
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

    void SetPlayerInput(PlayerAvatar player, PlayerInputMenu input)
    {
        player.GetComponent<PlayerInputMenu>().controller = input.controller;
        player.GetComponent<PlayerInputMenu>().id = input.id;
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

    PlayerAvatar SwitchPlayersInputs(PlayerAvatar player, PlayerInputMenu inputPlayer)
    {
        int ID = inputPlayer.id;
        PlayerInputMenu tempInput = player.GetComponent<PlayerInputMenu>();
        PlayerAvatar newAvatar = new PlayerAvatar();
        foreach (var input in playerInputs)
        {
            if (!playersInGame.Contains(input) && input.id != ID)
            {
                input.controller = inputPlayer.controller;
                input.id = inputPlayer.id;
                tempInput = input;
                newAvatar = input.GetComponent<PlayerAvatar>();
                break;
            }
        }
        newAvatar.GetComponent<PlayerInputMenu>().controller = tempInput.controller;
        newAvatar.GetComponent<PlayerInputMenu>().id = tempInput.id;
        return newAvatar;
    }
}
