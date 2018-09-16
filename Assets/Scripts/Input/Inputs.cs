using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {

    string actionButton_J1;
    string actionButton_J2;
    string actionButton_J3;
    string actionButton_J4;

    string actionButton_K1;
    string actionButton_K2;
    string actionButton_K3;

    string rejectButton_J1;
    string rejectButton_J2;
    string rejectButton_J3;
    string rejectButton_J4;

    string rejectButton_K1;
    string rejectButton_K2;
    string rejectButton_K3;

    void Start ()
    {
        SetUpActionsButtons();
    }
	
	void Update ()
    {
        if (Input.GetButtonDown(actionButton_J1))
            SetPlayerInput(0, 1);
        if (Input.GetButtonDown(actionButton_J2))
            SetPlayerInput(0, 2);
        if (Input.GetButtonDown(actionButton_J3))
            SetPlayerInput(0, 3);
        if (Input.GetButtonDown(actionButton_J4))
            SetPlayerInput(0, 4);

        if (Input.GetButtonDown(actionButton_K1))
            SetPlayerInput(1, 1);
        if (Input.GetButtonDown(actionButton_K2))
            SetPlayerInput(1, 2);
        if (Input.GetButtonDown(actionButton_K3))
            SetPlayerInput(1, 3);


        if (Input.GetButtonDown(rejectButton_J1))
            DisconnectPlayer(0, 1);
        if (Input.GetButtonDown(rejectButton_J2))
            DisconnectPlayer(0, 2);
        if (Input.GetButtonDown(rejectButton_J3))
            DisconnectPlayer(0, 3);
        if (Input.GetButtonDown(rejectButton_J4))
            DisconnectPlayer(0, 4);

        if (Input.GetButtonDown(rejectButton_K1))
            DisconnectPlayer(1, 1);
        if (Input.GetButtonDown(rejectButton_K2))
            DisconnectPlayer(1, 2);
        if (Input.GetButtonDown(rejectButton_K3))
            DisconnectPlayer(1, 3);
    }

    void SetUpActionsButtons()
    {
        actionButton_J1 =  "J_JumpButton_P1";
        actionButton_J2 =  "J_JumpButton_P2";
        actionButton_J3 =  "J_JumpButton_P3";
        actionButton_J4 =  "J_JumpButton_P4";

        actionButton_K1 =  "K_JumpButton_P1";
        actionButton_K2 =  "K_JumpButton_P2";
        actionButton_K3 =  "K_JumpButton_P3";

        rejectButton_J1 = "J_DownAttack_P1";
        rejectButton_J2 = "J_DownAttack_P2";
        rejectButton_J3 = "J_DownAttack_P3";
        rejectButton_J4 = "J_DownAttack_P4";

        rejectButton_K1 = "K_DownAttack_P1";
        rejectButton_K2 = "K_DownAttack_P2";
        rejectButton_K3 = "K_DownAttack_P3";
    }

    void SetPlayerInput(int controller, int ID)
    {
        PlayerInputMenu player = new PlayerInputMenu();
        player.controller = (PlayerInputMenu.Controller)controller;
        player.id = ID;
        SelectorPlayerManager.Instance.OnConnectedPlayer(player);
    }

    void DisconnectPlayer(int controller, int ID)
    {
        PlayerInputMenu player = new PlayerInputMenu();
        player.controller = (PlayerInputMenu.Controller)controller;
        player.id = ID;
        SelectorPlayerManager.Instance.OnDisconnectedPlayer(player);
    }
}
