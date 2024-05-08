using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [Header("---- Input ----")]
    [SerializeField] PlayerInput playerInput;
    [Header("---- Audio Data ----")]
    [SerializeField]AudioData pauseSFX;
    [SerializeField]AudioData unPauseSFX;

    [Header("---- Canvas ----")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menuCanvas;

    [Header("---- Button ----")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }
    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }
    void Pause()
    {
        hUDCanvas.enabled = false;
        menuCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    public void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(unPauseSFX);

    }

    void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;
        menuCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGamePlayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }
    void OnMainMenuButtonClick()
    {
        menuCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
