using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text roomName = default;
    [SerializeField] bool printDebug = false;
    [SerializeField] bool spawnPlayerEntity = false;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
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

    private void SpawnPlayer(Player player)
    {
        PhotonNetwork.Instantiate(playerEntityPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnConnectedToMaster()
    {
        if (printDebug) Debug.Log("connected to PUN");
        connectEvent.Raise();
    }

    public override void OnLeftRoom()
    {
        if (printDebug) Debug.Log("left room");
        roomName.text = "";
        leaveRoomEvent.Raise();
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (printDebug) Debug.Log(otherPlayer.NickName + " has left the room");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (printDebug) Debug.Log("disconnected from server");
        disconnectEvent.Raise();
    }

    public override void OnCreatedRoom()
    {
        if (printDebug) Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (printDebug) Debug.Log("create room failed: " + message);
        UIMessageDisplay.instance.DisplayMessage("create room failed: " + message);
    }

    public override void OnJoinedRoom()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (printDebug) Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
        joinRoomEvent.Raise();
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);

        if (spawnPlayerEntity)
        {
            SpawnPlayer(PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (printDebug) Debug.Log(newPlayer.NickName + "has joined the room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (printDebug) Debug.Log("join room failed: " + message);
        UIMessageDisplay.instance.DisplayMessage("join room failed: " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (printDebug) Debug.Log("join random failed");
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
}
