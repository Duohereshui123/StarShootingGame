using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Input")]

public class PlayerInput :
    ScriptableObject,
    PlayerInputActions.IGamePlayActions,
    PlayerInputActions.IPauseMenuActions,
    PlayerInputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnpause = delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };

    PlayerInputActions inputActions;

    void OnEnable()
    {
        inputActions = new PlayerInputActions();

        //登记回调函数
        inputActions.GamePlay.SetCallbacks(this);//GamePlay is in InputActions's Action Maps
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInput();
    }

    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;//mouse invisible
            Cursor.lockState = CursorLockMode.Locked;//mouse move locked
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInput() => inputActions.Disable();
    public void EnableGamePlayInput() => SwitchActionMap(inputActions.GamePlay, false);
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);
    public void EnableGameOverScreenInput() =>SwitchActionMap(inputActions.GameOverScreen, false);
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)//使用属性 更简洁
        {
            onFire.Invoke();
        }

        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
