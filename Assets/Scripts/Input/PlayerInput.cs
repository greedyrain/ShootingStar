using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "ScriptableObject/PlayerInput")]
public class PlayerInput : ScriptableObject, PlayerInputAction.IGamePlayActions
{
    private PlayerInputAction inputAction;
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStop = delegate { };

    public event UnityAction onFire = delegate { };
    public event UnityAction onFireStop = delegate { };

    private void OnEnable()
    {
        inputAction = new PlayerInputAction();
        inputAction.GamePlay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableGamePlayInput();
    }

    public void EnableGameplay()
    {
        inputAction.GamePlay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableGamePlayInput()
    {
        inputAction.GamePlay.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            onStop();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFire();
        }
        
        if (context.canceled)
        {
            onFireStop();
        }
    }
}