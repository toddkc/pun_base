namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using UnityEngine;

    public class UI_RandomGame : MonoBehaviour
    {
        [SerializeField] GameEvent joinRandomEvent = default;

        public void RandomGame()
        {
            joinRandomEvent.Raise();
            CustomNetworkManager.instance.CreateOrJoinRandomRoom();
        }
    }
}