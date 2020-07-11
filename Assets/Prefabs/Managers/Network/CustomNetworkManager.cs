using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerEntityPrefab = default;
    [SerializeField] GameEvent joinRoomEvent = default;
    [SerializeField] GameEvent leaveRoomEvent = default;
    [SerializeField] GameEvent disconnectEvent = default;
    [SerializeField] GameEvent connectEvent = default;

    public static CustomNetworkManager instance;

    private int counter = 0;
    private bool joiningRandom = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // create a new room with a specific name
    public void CreateRoom(string roomname)
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnected) return;
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.CreateRoom(roomname);
    }

    // join a room with a specific name
    public void JoinRoom(string roomname)
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnected) return;
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.JoinRoom(roomname);
    }

    // attempt to join any room or else create one
    public void CreateOrJoinRandomRoom()
    {
        counter = 0;
        if (!PhotonNetwork.IsConnected) return;
        PhotonNetwork.JoinRandomRoom();
        joiningRandom = true;
    }

    // user-triggered leaving the current room
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // user-triggered PUN disconnect
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }


    #region Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to PUN");
        connectEvent.Raise();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        AudioPlayer.PlayerLeft();
        leaveRoomEvent.Raise();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left the room");
        AudioPlayer.PlayerLeft();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconnected from server");
        disconnectEvent.Raise();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
        AudioPlayer.PlayerJoined();
        joinRoomEvent.Raise();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "has joined the room");
        AudioPlayer.PlayerJoined();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("join random failed");
        if (joiningRandom)
        {
            if (counter < 10)
            {
                counter++;
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                joiningRandom = false;
                PhotonNetwork.CreateRoom(null);
            }
        }
    }

    #endregion
}
