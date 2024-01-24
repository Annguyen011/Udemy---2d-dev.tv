using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputActions playerInputActions;
    private InputAction _move;
    private InputAction _jump;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        _move = playerInputActions.Player.Move;
        _jump = playerInputActions.Player.Jump;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        FrameInput= GatherInput();
    }

    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            move = this._move.ReadValue<Vector2>(),
            Jump = this._jump.WasPerformedThisFrame(),
        };
    }
}

public struct FrameInput
{
    public Vector2 move;
    public bool Jump;
}
