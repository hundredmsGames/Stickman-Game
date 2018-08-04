using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    //panel active events
    public delegate void PanelActiveHandler(bool active);
    public event PanelActiveHandler FailedPanelActiveChanged;
    public event PanelActiveHandler FinishedPanelActiveChanged;
    public event PanelActiveHandler PausedPanelActiveChanged;

    //Button events
    public delegate void ButtonHandler(object info,UnityEngine.Events.UnityAction function);
    public event ButtonHandler PauseResumeButtonClicked;

    public CharacterController character;
    public DrawLine2D drawLine;

    public bool gamePaused;

    // Use this for initialization
    void Awake ()
    {
        if (Instance != null)
            return;

        Instance = this;
        
	}
    public void Failed()
    {
        FailedPanelActiveChanged(true);
        gamePaused = true;
        Time.timeScale = 0;
    }
    public void PauseGame()
    {
        PausedPanelActiveChanged(true);
        PauseResumeButtonClicked(null, ResumeGame);
        gamePaused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        PausedPanelActiveChanged(false);
        PauseResumeButtonClicked(null, PauseGame);
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        FinishedPanelActiveChanged(false);
        FailedPanelActiveChanged(false);
        character.ResetCharacter();
        drawLine.Reset();
        gamePaused = false;
    }

    // Find better names for this methods.
    public void FinishedGame()
    {
        FinishedPanelActiveChanged(true);
        gamePaused = true;
        Time.timeScale = 0;
    }
}
