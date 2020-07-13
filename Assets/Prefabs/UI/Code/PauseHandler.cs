using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    // if in a game hide the leave room and change game buttons
    // if pause button pressed toggle those buttons on/off

    [SerializeField] KeyCode pauseKey = default;
    [SerializeField] List<GameObject> panelsToToggle = new List<GameObject>();
    [SerializeField] List<GameObject> panelsToToggleMaster = new List<GameObject>();
    private bool inGame = false;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey) && inGame)
        {
            UpdatePanels();
        }
    }

    private void UpdatePanels()
    {
        isPaused = !isPaused;
        foreach(var panel in panelsToToggle)
        {
            panel.SetActive(isPaused);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var panel in panelsToToggleMaster)
            {
                panel.SetActive(isPaused);
            }
        }
    }

    public void UpdateInGame(bool active)
    {
        inGame = active;
    }
}
