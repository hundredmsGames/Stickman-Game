using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameController gameController;
    public GameObject panelFailed;
    public GameObject panelFinished;

    private void Start()
    {
        gameController = GameController.Instance;

        gameController.RegisterToggleFailedPanel(SetFailedPanel);
        gameController.RegisterFinishedPanel(SetFinishedPanel);

    }

    public void SetFailedPanel(bool active)
    {
        panelFailed.SetActive(active);
    }

    public void SetFinishedPanel(bool active)
    {
        panelFinished.SetActive(active);
    }


}
