
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ActionManager : MonoBehaviour
{
    public UnityEvent<bool> jump;
    public UnityEvent<bool> attack;
    public UnityEvent<bool> jumpHold;
    public UnityEvent<int> move;

    public void OnJumpHoldAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Debug.Log("JumpHold was performed");
            // Debug.Log(context.duration);
            jumpHold.Invoke(true);
        }
    }

    // called twice, when pressed and unpressed
    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.started) { jump.Invoke(true); }
        else if (context.canceled) { jump.Invoke(false); }

    }

    public void OnAttackAction(InputAction.CallbackContext context)
    {
        if (context.started) { attack.Invoke(true); }
        else if (context.canceled) { attack.Invoke(false); }

    }

    // called twice, when pressed and unpressed
    public void OnMoveAction(InputAction.CallbackContext context)
    {
        // Debug.Log("OnMoveAction callback invoked");
        if (context.started)
        {
            // Debug.Log("move started");
            int dir = context.ReadValue<float>() > 0 ? 1 : -1;
            move.Invoke(dir);
        }
        if (context.performed)
        {
            // Debug.Log("move performed");
            int dir = context.ReadValue<float>() > 0 ? 1 : -1;
            move.Invoke(dir);
        }
        if (context.canceled)
        {
            // Debug.Log("move stopped");
            move.Invoke(0);
        }

    }
}
