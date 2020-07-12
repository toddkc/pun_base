using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    private VoiceConnection voiceConnection;

    [SerializeField]
    private bool usePhotonRoom = true;
    [SerializeField]
    private string roomName;

    private readonly EnterRoomParams enterRoomParams = new EnterRoomParams
    {
        RoomOptions = new RoomOptions()
    };

    public bool IsConnected { get { return voiceConnection.Client.IsConnected; } }

    private void Awake()
    {
        voiceConnection = GetComponent<VoiceConnection>();
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

    public void JoinVoiceRoom()
    {
        //enterRoomParams.RoomName = roomName;
        enterRoomParams.RoomName = PhotonNetwork.CurrentRoom.Name + "_voice";
        Debug.Log(enterRoomParams.RoomName);
        voiceConnection.Client.OpJoinOrCreateRoom(enterRoomParams);
        voiceConnection.PrimaryRecorder.TransmitEnabled = true;
    }
    public void LeaveVoiceRoom()
    {
        voiceConnection.Client.OpLeaveRoom(false);
    }

    public void OnConnectedToMaster(){}
    public void OnJoinedRoom(){}
    public void OnLeftRoom(){}
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
