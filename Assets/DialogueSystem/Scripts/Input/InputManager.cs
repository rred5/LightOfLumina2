using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Vector2 moveDirection = Vector2.zero;
    private bool jumpPressed = false;
    private bool interactPressed = false;
    private bool submitPressed = false;
    public bool pausePressed = false;
    public bool bPressed = false;

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public static InputManager GetInstance() 
    {
        return instance;
    }
    public void PausePressed(InputAction.CallbackContext context)
    {
        if (context.started) {
            pausePressed = true;
        }
        /*
        if (context.performed)
        {
            pausePressed = true;
        }

        
        else if (context.canceled)
        {
            pausePressed = false;
        }
        */
        
    }

    public void BPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            bPressed = true;
        }
        /*
        if (context.performed)
        {
            pausePressed = true;
        }

        
        else if (context.canceled)
        {
            pausePressed = false;
        }
        */

    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        } 
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        } 
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        } 
    }

    public Vector2 GetMoveDirection() 
    {
        return moveDirection;
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetJumpPressed() 
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed() 
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public bool GetPausePressed()
    {
        bool result = pausePressed;
       // pausePressed = false;
        return result;
        
    }

    public bool GetBPressed() {
        bool result = bPressed;
        //bPressed = false;
        return result;

            }

    public void SetPausePressed(bool pauseCheck) {
        pausePressed = pauseCheck;
    }

    public void SetBPressed(bool BCheck)
    {
       bPressed = BCheck;
    }


    public void RegisterSubmitPressed() 
    {
        submitPressed = false;
    }

}
