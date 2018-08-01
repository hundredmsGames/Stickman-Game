using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public CharacterController character;
    public DrawLine2D drawLine;

    public bool gamePaused;

    Action<bool> setFailedPanel;
    Action<bool> setFinishedPanel;


    // Use this for initialization
    void Awake ()
    {
        if (Instance != null)
            return;

        Instance = this;
	}

    public void PauseGame()
    {
        setFailedPanel(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        setFinishedPanel(false);
        setFailedPanel(false);
        character.ResetCharacter();
        drawLine.Reset();
        gamePaused = false;
    }

    // Find better names for this methods.
    public void FinishedGame()
    {
        setFinishedPanel(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void RegisterToggleFailedPanel(Action<bool> cb)
    {
        setFailedPanel += cb;
    }

    public void RegisterFinishedPanel(Action<bool> cb)
    {
        setFinishedPanel += cb;
    }
}
