namespace NMT
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;

    public class CustomNetworkManager : MonoBehaviourPunCallbacks
    {
        public static CustomNetworkManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private bool printDebug = true;

        /// <summary>
        /// called when the game makes a connection to the PUN servers
        /// </summary>
        public override void OnConnectedToMaster()
        {
            if (printDebug) Debug.Log("connected to PUN");
        }

        /// <summary>
        /// called when the game disconnects from the PUN servers
        /// </summary>
        /// <param name="cause"></param>
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (printDebug) Debug.Log("disconnected from server");
        }

        /// <summary>
        /// called when the local player joins a room
        /// </summary>
        public override void OnJoinedRoom()
        {
            if (printDebug) Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
        }

        /// <summary>
        /// called when the local player leaves a room
        /// </summary>
        public override void OnLeftRoom()
        {
            if (printDebug) Debug.Log("left room");
        }

        /// <summary>
        /// called when the local player fails to join a specific room
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("join room failed: " + message);
        }

        /// <summary>
        /// called when the local player fails to join a random room
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("join random failed");
        }

        /// <summary>
        /// called when a network player joins the current room
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (printDebug) Debug.Log(newPlayer.NickName + "has joined the room");
        }

        /// <summary>
        /// called when a network player leaves the current room
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (printDebug) Debug.Log(otherPlayer.NickName + " has left the room");
        }

        /// <summary>
        /// called when we create a new room
        /// </summary>
        public override void OnCreatedRoom()
        {
            if (printDebug) Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
        }

        /// <summary>
        /// called when creating a room fails
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("create room failed: " + message);
        }
    }
}