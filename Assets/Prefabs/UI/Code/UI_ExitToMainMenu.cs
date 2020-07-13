using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ExitToMainMenu : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex = 0;
    public void ExitToMainMenu()
    {
        Debug.Log("Exit To Main Menu");
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
