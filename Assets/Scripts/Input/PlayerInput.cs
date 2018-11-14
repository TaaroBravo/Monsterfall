﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInput : MonoBehaviour
{
    public int player_number;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    public Controller controller;
    public int id;

    PlayerController player;
    public string horizontalMove;
    public string verticalMove;
    public string jumpButton;
    public string normalAttack;
    public string downAttack;
    public string upAttack;
    public string hability1Button;
    public string hability2Button;
    public string hability3Button;

    public enum Controller
    {
        J,
        K
    }

    private void Start()
    {
        player = GetComponent<PlayerController>();
        SetPlayerInput();
    }

    void Update()
    {
        playerIndex = (PlayerIndex)id - 1;
        SetPlayerInput();
        if (player && player.canMove && !GameManager.Instance.startingGame && !GameManager.Instance.finishedGame)
        {
            if (controller == Controller.J)
            {
                prevState = state;
                state = GamePad.GetState(playerIndex);
                if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
                    player.Jump();

                if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
                    player.AttackDown("Pressed");

                if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
                    player.AttackDown("Realese");

                if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
                    player.AttackNormal("Pressed");

                if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
                    player.AttackNormal("Realese");

                if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
                    player.AttackUp("Pressed");

                if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
                    player.AttackUp("Realese");

                if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
                    player.Dash();

                if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
                    player.Hability("Pressed");

                if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released)
                    player.Hability("Realese");

                //if (Mathf.Round(Input.GetAxisRaw(hability3Button)) < 0)
                //    player.FallOff();
            }
            //if (Input.GetButtonDown(jumpButton))
            //    player.Jump();
            //if (Input.GetButtonDown(hability1Button))
            //    player.Dash();
            //if (Input.GetButtonDown(hability2Button))
            //    player.Hability("Pressed");
            //if (Input.GetButtonUp(hability2Button))
            //    player.Hability("Realese");
            //if (Input.GetButtonDown(hability3Button))
            //    player.FallOff();
            //if (controller == Controller.J)
            //{
            //    if (Mathf.Round(Input.GetAxisRaw(hability3Button)) < 0)
            //        player.FallOff();
            //}
            //if (Input.GetButtonDown(normalAttack))
            //    player.AttackNormal("Pressed");
            //if (Input.GetButtonUp(normalAttack))
            //    player.AttackNormal("Realese");
            //if (Input.GetButtonDown(downAttack))
            //    player.AttackDown("Pressed");
            //if (Input.GetButtonUp(downAttack))
            //    player.AttackDown("Realese");
            //if (Input.GetButtonDown(upAttack))
            //    player.AttackUp("Pressed");
            //if (Input.GetButtonUp(upAttack))
            //    player.AttackUp("Realese");
        }
    }

    public float MainHorizontal()
    {
        float r = 0.0f;
        if (controller == Controller.J)
            r += state.ThumbSticks.Left.X;
        else
            r += Input.GetAxis(controller.ToString() + "_MainHorizontal_P" + id);
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public float MainVertical()
    {
        float r = 0.0f;
        if (controller == Controller.J)
            r += state.ThumbSticks.Left.Y;
        else
            r += Input.GetAxis(controller.ToString() + "_MainVertical_P" + id);
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public void SetPlayerInput()
    {
        horizontalMove = controller.ToString() + "_MainHorizontal_P" + id;
        verticalMove = controller.ToString() + "_MainVertical_P" + id;
        jumpButton = controller.ToString() + "_JumpButton_P" + id;
        normalAttack = controller.ToString() + "_NormalAttack_P" + id;
        downAttack = controller.ToString() + "_DownAttack_P" + id;
        upAttack = controller.ToString() + "_UpAttack_P" + id;
        hability1Button = controller.ToString() + "_Hability1_P" + id;
        hability2Button = controller.ToString() + "_Hability2_P" + id;
        hability3Button = controller.ToString() + "_Hability3_P" + id;
    }
}
