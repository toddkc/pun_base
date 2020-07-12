using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    private VoiceConnection voice;

    private void Awake()
    {
        voice = GetComponent<VoiceConnection>();
    }

    private void Start()
    {
        voice.ConnectUsingSettings();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        voice.Client.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        voice.Client.RemoveCallbackTarget(this);
    }

    public void EnableVoiceChat()
    {
        var room = new EnterRoomParams();
        room.RoomName = PhotonNetwork.CurrentRoom.Name + "_voice";
        voice.Client.OpJoinOrCreateRoom(room);
    }

    public void DisableVoiceChat()
    {
        voice.Client.OpLeaveRoom(false);
    }

    public override void OnJoinedRoom()
    {
        if(voice.Client != null && voice.Client.InRoom)
        {
            Debug.Log("voice chat joined room: " + voice.Client.CurrentRoom.Name);
        }
    }

    public override void OnLeftRoom()
    {
        if (voice.Client != null)
        {
            Debug.Log("voice chat left room");
        }
    }
}
