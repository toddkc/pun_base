using UnityEngine;
using UnityEngine.UI;

public class UI_HostGame : MonoBehaviour
{
    [SerializeField] InputField roomNameInput = default;

    public void HostGame()
    {
        string roomname = roomNameInput.text;
        if (string.IsNullOrEmpty(roomname)) return;
        CustomNetworkManager.instance.CreateRoom(roomname);
    }
}
