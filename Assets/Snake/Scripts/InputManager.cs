using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> onMove;
    public event Action<bool> onJump;
    private InputSystem_Actions inputs;
    private Vector2 moveInput;
    private bool jumpInput; 
 
    private void OnEnable()
    {
        SetupInput();
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void SetupInput()
    {
        inputs = new InputSystem_Actions();


        inputs.Player.Move.performed += OnMove;
        inputs.Player.Jump.performed += OnJump;
        
    }

    private void EnableInput()
    {
        inputs.Player.Enable();
     
    }

    private void DisableInput()
    {
        inputs.Player.Disable();
      
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        onMove?.Invoke(moveInput);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.ReadValueAsButton();

        onJump?.Invoke(jumpInput);
    }
  
    private void Update()
    {
        if (onMove != null)
        {
            onMove(moveInput);
        }
    }

}
