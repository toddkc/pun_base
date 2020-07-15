namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using UnityEngine;

    public class UI_LeaveRoom : MonoBehaviour
    {
        [SerializeField] GameEvent leaveRoomEvent = default;

        public void LeaveRoom()
        {
            leaveRoomEvent.Raise();
            CustomNetworkManager.instance.LeaveRoom();
        }
    }
}