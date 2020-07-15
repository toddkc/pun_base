namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using Photon.Pun;
    using Photon.Realtime;
    using Photon.Voice.Unity;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class VoiceManager : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
    {
        private VoiceConnection voiceConnection;

        [SerializeField]
        private bool enableVoice = false;
        [SerializeField]
        private bool printDebug = false;
        [SerializeField]
        private GameEvent connectedEvent = default;
        [SerializeField]
        private Text roomName = default;

        public bool IsConnected { get { return voiceConnection.Client.IsConnected; } }

        private void Awake()
        {
            voiceConnection = GetComponent<VoiceConnection>();
            voiceConnection.Settings.AppVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
            voiceConnection.Settings.AppIdVoice = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;
        }

        private void Start()
        {
            voiceConnection.ConnectUsingSettings();
        }

        private void OnEnable()
        {
            voiceConnection.Client.AddCallbackTarget(this);
        }
        private void OnDisable()
        {
            voiceConnection.Client.RemoveCallbackTarget(this);
        }

        //private void Update()
        //{
        //    if (printDebug) GetPlayers();
        //}

        private void GetPlayers()
        {
            if (!voiceConnection.Client.InRoom) return;
            string players = "players in voice room: ";
            foreach (var player in voiceConnection.Client.CurrentRoom.Players)
            {
                players += " || " + player.Value.NickName + " || ";
            }
            Debug.Log(players);
        }

        public void EnableVoice()
        {
            if (!enableVoice) return;
            voiceConnection.Client.NickName = PhotonNetwork.NickName;
            string room = PhotonNetwork.CurrentRoom.Name + "_voice";
            var roomParams = new EnterRoomParams
            {
                RoomName = room
            };

            if (PhotonNetwork.IsMasterClient)
            {
                voiceConnection.Client.OpCreateRoom(roomParams);
            }
            else
            {
                voiceConnection.Client.OpJoinRoom(roomParams);
            }
        }

        public void DisableVoice()
        {
            if (voiceConnection.Client.InRoom)
            {
                voiceConnection.Client.OpLeaveRoom(false);
            }
        }

        public void OnConnectedToMaster()
        {
            if (printDebug) Debug.Log("voice connected to server");
            connectedEvent.Raise();
        }

        public void OnJoinedRoom()
        {
            voiceConnection.PrimaryRecorder.TransmitEnabled = true;
            roomName.text = voiceConnection.Client.CurrentRoom.Name;
            if (printDebug) Debug.Log("voice room joined: " + voiceConnection.Client.CurrentRoom.Name);
        }

        public void OnLeftRoom()
        {
            voiceConnection.PrimaryRecorder.TransmitEnabled = false;
            roomName.text = "";
        }

        public void OnCreatedRoom() { }
        public void OnCreateRoomFailed(short returnCode, string message) { }
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }
        public void OnJoinRandomFailed(short returnCode, string message) { }
        public void OnJoinRoomFailed(short returnCode, string message) { }
        public void OnConnected() { }
        public void OnDisconnected(DisconnectCause cause) { }
        public void OnRegionListReceived(RegionHandler regionHandler) { }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public void OnCustomAuthenticationFailed(string debugMessage) { }
    }
}