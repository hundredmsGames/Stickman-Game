using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public GameController gameController;
    public GameObject panelFailed;
    public GameObject panelFinished;

    public GameObject panelPaused;
    public Button pauseResumeButton;
    private Text pauseText;


    private void Start()
    {
        gameController = GameController.Instance;

        pauseText = pauseResumeButton.GetComponentInChildren<Text>();

        gameController.FailedPanelActiveChanged += SetFailedPanel;
        gameController.FinishedPanelActiveChanged += SetFinishedPanel;
        gameController.PausedPanelActiveChanged += SetPausedPanel;
        gameController.PauseResumeButtonClicked += GameController_PauseResumeButtonClicked;
    }

    private void GameController_PauseResumeButtonClicked(object info, UnityEngine.Events.UnityAction function)
    {
        if(info is string)
        pauseText.text = (string)info;
        pauseResumeButton.onClick.RemoveAllListeners();
        pauseResumeButton.onClick.AddListener(function);
    }

    public void SetFailedPanel(bool active)
    {
        panelFailed.SetActive(active);
    }

    public void SetFinishedPanel(bool active)
    {
        panelFinished.SetActive(active);
    }

    public void SetPausedPanel(bool active)
    {
        panelPaused.SetActive(active);

    }

   
}
