using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInputMenu : MonoBehaviour
{

    PlayerAvatar player;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    public Controller controller;
    public int id;

    public string horizontalMove;
    public string verticalMove;
    public string actionButton;
    public string rejectButton;

    bool _cooldown;

    public enum Controller
    {
        J,
        K
    }

    void Start()
    {
        player = GetComponent<PlayerAvatar>();
        //StartCoroutine(CoolDown());
        _cooldown = true;
    }

    private void LateUpdate()
    {
        //if (id < 1)
        //{
        //    CleanInputs();
        //    return;
        //}
        playerIndex = (PlayerIndex)id - 1;
        SetPlayerInput();
        if (controller == Controller.J)
        {
            prevState = state;
            state = GamePad.GetState(playerIndex);
            if (state.ThumbSticks.Left.X != 0 && _cooldown)
            {
                player.Move(new Vector2(state.ThumbSticks.Left.X == 0 ? 0 : state.ThumbSticks.Left.X > 0 ? 1 : -1, 0));
                StartCoroutine(CoolDown());
            }

            if (state.ThumbSticks.Left.Y != 0 && _cooldown)
            {
                player.Move(new Vector2(0, state.ThumbSticks.Left.Y == 0 ? 0 : state.ThumbSticks.Left.Y > 0 ? -1 : 1));
                StartCoroutine(CoolDown());
            }

            if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
                player.RejectButton();

            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
                player.ActionButton();

        }
        //if (Input.GetButtonDown(actionButton))
        //    player.ActionButton();
        //if (Input.GetButtonDown(rejectButton))
        //    player.RejectButton();
        //if (controller == Controller.J)
        //{
        //    if (Input.GetAxis(horizontalMove) != 0 && _cooldown)
        //    {
        //        player.Move(new Vector2(Input.GetAxis(horizontalMove) == 0 ? 0 : Input.GetAxis(horizontalMove) > 0 ? 1 : -1, 0));
        //        StartCoroutine(CoolDown());
        //    }
        //    if (Input.GetAxis(verticalMove) != 0 && _cooldown)
        //    {
        //        player.Move(new Vector2(0, Input.GetAxis(verticalMove) == 0 ? 0 : Input.GetAxis(verticalMove) > 0 ? 1 : -1));
        //        StartCoroutine(CoolDown());
        //    }
        //}
        //else
        //{
        //    if (Input.GetButtonDown(horizontalMove))
        //        player.Move(new Vector2((int)MainHorizontal(), 0));
        //    if (Input.GetButtonDown(verticalMove))
        //        player.Move(new Vector2(0, -(int)MainVertical()));
        //}
    }

    IEnumerator CoolDown()
    {
        while (true)
        {
            //yield return new WaitUntil(() => !_cooldown);
            _cooldown = false;
            yield return new WaitForSeconds(0.2f);
            _cooldown = true;
            break;
        }
    }

    public float MainHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis(controller.ToString() + "_MainHorizontal_P" + id);
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public float MainVertical()
    {
        float r = 0.0f;
        r += Input.GetAxis(controller.ToString() + "_MainVertical_P" + id);
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public void SetPlayerInput()
    {
        if (controller == Controller.J)
        {
            horizontalMove = "JoystickHorizontal_P" + id;
            verticalMove = "JoystickVertical_P" + id;
        }
        else
        {
            horizontalMove = controller.ToString() + "_MainHorizontal_P" + id;
            verticalMove = controller.ToString() + "_MainVertical_P" + id;
        }
        if (controller == Controller.K && id == 2)
            actionButton = controller.ToString() + "_NormalAttack_P" + id;
        else
            actionButton = controller.ToString() + "_JumpButton_P" + id;
        rejectButton = controller.ToString() + "_DownAttack_P" + id;
    }

    void CleanInputs()
    {
        horizontalMove = "";
        verticalMove = "";
        actionButton = "";
        rejectButton = "";
    }
}
