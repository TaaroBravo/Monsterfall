using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{

    PlayerAvatar player;

    public Controller controller;
    public int id;

    public string horizontalMove;
    public string verticalMove;
    public string actionButton;
    public string rejectButton;

    public enum Controller
    {
        J,
        K
    }

    void Start()
    {
        player = GetComponent<PlayerAvatar>();
    }

    private void LateUpdate()
    {
        if (id < 1)
        {
            CleanInputs();
            return;
        }

        SetPlayerInput();
        if (Input.GetButtonDown(actionButton))
            player.ActionButton();
        if (Input.GetButtonDown(rejectButton))
            player.RejectButton();
        if (controller == Controller.J)
        {
            if (Input.GetAxis(horizontalMove) != 0)
                player.Move((int)MainHorizontal());
            if (Input.GetAxis(verticalMove) != 0)
                player.Move(-(int)MainVertical() * 3);
        }
        else
        {
            if (Input.GetButtonDown(horizontalMove))
                player.Move((int)MainHorizontal());
            if (Input.GetButtonDown(verticalMove))
                player.Move(-(int)MainVertical() * 3);
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
        horizontalMove = controller.ToString() + "_MainHorizontal_P" + id;
        verticalMove = controller.ToString() + "_MainVertical_P" + id;
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
