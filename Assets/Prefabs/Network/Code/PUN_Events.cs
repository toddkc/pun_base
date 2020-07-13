using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class PUN_Events
{
    public const byte LoadLevelEventCode = 1;
    public const byte UnloadLevelEventCode = 2;

    public static void LoadLevelEvent(int buildIndex)
    {
        object[] content = new object[] { buildIndex };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(LoadLevelEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void UnloadLevelEvent()
    {
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(UnloadLevelEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
