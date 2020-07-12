using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private GameObject menuCam;

    private void Start()
    {
        if(menuCam == null)
        {
            Debug.LogError("no main menu camera has been assigned", this);
        }
    }

    public void ActivateMenuCamera(bool active)
    {
        menuCam.SetActive(active);
    }
}
