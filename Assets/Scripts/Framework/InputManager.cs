using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Variables
    public static InputManager instance;
    
    public KeyCode jump = KeyCode.W;
    public KeyCode crouch = KeyCode.S;
    public KeyCode punch = KeyCode.J;
    public KeyCode enableUltimate = KeyCode.I;

    public KeyCode punchGamepad = KeyCode.JoystickButton2;
    public KeyCode jumpGamepad = KeyCode.JoystickButton0;
    public KeyCode enableUltimateGamepad = KeyCode.JoystickButton3;
    #endregion

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    #region Keyboard
    public float GetHorizontalMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    public bool GetJump()
    {
        return Input.GetKeyDown(jump);
    }

    public bool GetCrouch()
    {
        return Input.GetKey(crouch);
    }

    public bool GetCrouchDown()
    {
        return Input.GetKeyDown(crouch);
    }

    public bool GetCrouchUp()
    {
        return Input.GetKeyUp(crouch);
    }

    public bool GetPunch()
    {
        return Input.GetKeyDown(punch);
    }

    public bool GetEnableUltimate()
    {
        return (Input.GetKeyDown(enableUltimate));
    }
    #endregion

    #region Joystick
    public float GetGamepadHorizontalMovement()
    {
        return Input.GetAxis("HorizontalGamepad");
    }

    public bool GetGamepadJump()
    {
        return Input.GetKeyDown(jumpGamepad);
    }

    public bool GetGamepadCrouch()
    {
        return Input.GetAxis("VerticalGamepad") <= -0.5 ? true : false;
    }

    public bool GetGamepadPunch()
    {
        return Input.GetKeyDown(punchGamepad);
    }

    public bool GetGamepadEnableUltimate()
    {
        return Input.GetKeyDown(enableUltimateGamepad);
    }
    #endregion

}
