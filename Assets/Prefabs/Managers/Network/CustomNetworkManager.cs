using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerEntityPrefab = default;

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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomname)
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnected) return;
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.CreateRoom(roomname);
    }

    public void JoinRoom(string roomname)
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnected) return;
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.JoinRoom(roomname);
    }

    public void CreateOrJoinRandomRoom()
    {
        counter = 0;
        if (!PhotonNetwork.IsConnected) return;
        PhotonNetwork.JoinRandomRoom();
        joiningRandom = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void LoadGameScene()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LoadMenuScene()
    {
        PhotonNetwork.LoadLevel(0);
    }

    //public void SpawnPlayerEntity(Player owner)
    //{
    //    if (!PhotonNetwork.IsMasterClient) return;
    //    var entity = PhotonNetwork.Instantiate(playerEntityPrefab.name, Vector3.zero, Quaternion.identity);
    //    entity.GetPhotonView().TransferOwnership(owner);
    //}

    #region Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        AudioPlayer.PlayerLeft();
        LoadMenuScene();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left the room");
        AudioPlayer.PlayerLeft();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconnected from server");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
        AudioPlayer.PlayerJoined();
        LoadGameScene();
        //SpawnPlayerEntity(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "has joined the room");
        AudioPlayer.PlayerJoined();
        //SpawnPlayerEntity(newPlayer);
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
