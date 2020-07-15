namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_JoinGame : MonoBehaviour
    {
        [SerializeField] InputField roomNameInput = default;
        [SerializeField] GameEvent joinRoomEvent = default;

        public void JoinGame()
        {
            string roomname = roomNameInput.text;
            if (string.IsNullOrEmpty(roomname)) return;
            joinRoomEvent.Raise();
            CustomNetworkManager.instance.JoinRoom(roomname);
        }
    }
}