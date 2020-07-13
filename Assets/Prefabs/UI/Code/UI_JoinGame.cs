using UnityEngine;
using UnityEngine.UI;

public class UI_JoinGame : MonoBehaviour
{
    [SerializeField] InputField roomNameInput = default;

    public void JoinGame()
    {
        string roomname = roomNameInput.text;
        if (string.IsNullOrEmpty(roomname)) return;
        CustomNetworkManager.instance.JoinRoom(roomname);
    }
}
