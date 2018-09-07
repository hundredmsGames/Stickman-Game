using UnityEngine;

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

    public TrapController trapController;
    public CharacterController character;

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
        trapController.DestroyTraps();
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
