using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")]
    [SerializeField] Canvas mainMenuCanvas;

    [Header("==== BUTTONS ====")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;
 

    private void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);

    }
    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
