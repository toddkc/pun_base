using NetworkTutorial.GameEvents;
using UnityEngine;

public class UI_QuitGame : MonoBehaviour
{
    [SerializeField] GameEvent quitEvent = default;

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        quitEvent.Raise();
        Application.Quit();
    }
}
