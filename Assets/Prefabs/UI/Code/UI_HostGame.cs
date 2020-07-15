namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_HostGame : MonoBehaviour
    {
        [SerializeField] InputField roomNameInput = default;
        [SerializeField] GameEvent createRoomEvent = default;

        public void HostGame()
        {
            string roomname = roomNameInput.text;
            if (string.IsNullOrEmpty(roomname)) return;
            createRoomEvent.Raise();
            CustomNetworkManager.instance.CreateRoom(roomname);
        }
    }
}