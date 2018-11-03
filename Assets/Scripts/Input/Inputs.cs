using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Inputs : MonoBehaviour {

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    PlayerIndex[] playerIndices;

    string startButton_J1;
    string startButton_J2;
    string startButton_J3;
    string startButton_J4;

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
        playerIndices = new PlayerIndex[4];
        SetUpActionsButtons();
    }
	
	void Update ()
    {
        GetGamepadInputs();

        if (Input.GetButtonDown(startButton_J1))
            SetPlayerInput(0, (int)playerIndices[0] + 1);
        if (Input.GetButtonDown(startButton_J2))
            SetPlayerInput(0, (int)playerIndices[1] + 1);
        if (Input.GetButtonDown(startButton_J3))
            SetPlayerInput(0, (int)playerIndices[2] + 1);
        if (Input.GetButtonDown(startButton_J4))
            SetPlayerInput(0, (int)playerIndices[3] + 1);

        if (Input.GetButtonDown(actionButton_K1))
            SetPlayerInput(1, 1);
        if (Input.GetButtonDown(actionButton_K2))
            SetPlayerInput(1, 2);
        if (Input.GetButtonDown(actionButton_K3))
            SetPlayerInput(1, 3);


        if (Input.GetButtonDown(rejectButton_J1))
            DisconnectPlayer(0, (int)playerIndices[0] + 1);
        if (Input.GetButtonDown(rejectButton_J2))
            DisconnectPlayer(0, (int)playerIndices[1] + 1);
        if (Input.GetButtonDown(rejectButton_J3))
            DisconnectPlayer(0, (int)playerIndices[2] + 1);
        if (Input.GetButtonDown(rejectButton_J4))
            DisconnectPlayer(0, (int)playerIndices[3] + 1);

        if (Input.GetButtonDown(rejectButton_K1))
            DisconnectPlayer(1, 1);
        if (Input.GetButtonDown(rejectButton_K2))
            DisconnectPlayer(1, 2);
        if (Input.GetButtonDown(rejectButton_K3))
            DisconnectPlayer(1, 3);
    }

    void SetUpActionsButtons()
    {
        startButton_J1 = "JoystickStart_P1";
        startButton_J2 = "JoystickStart_P2";
        startButton_J3 = "JoystickStart_P3";
        startButton_J4 = "JoystickStart_P4";

        actionButton_J1 =  "J_JumpButton_P1";
        actionButton_J2 =  "J_JumpButton_P2";
        actionButton_J3 =  "J_JumpButton_P3";
        actionButton_J4 =  "J_JumpButton_P4";

        actionButton_K1 =  "K_JumpButton_P1";
        actionButton_K2 =  "K_NormalAttack_P2";
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
        SelectorPlayerManager.Instance.OnConnectedPlayer(player, ID);
    }

    void DisconnectPlayer(int controller, int ID)
    {
        PlayerInputMenu player = new PlayerInputMenu();
        player.controller = (PlayerInputMenu.Controller)controller;
        player.id = ID;
        SelectorPlayerManager.Instance.OnDisconnectedPlayer(player);
    }

    void GetGamepadInputs()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndices[i] = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }
}
