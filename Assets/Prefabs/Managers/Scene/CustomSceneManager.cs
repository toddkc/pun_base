using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CustomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerEntity = default;

    // need to disconnect from chat and voice when leaving?

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            SpawnPlayerEntity(player.Value);
        }
    }

    public void SpawnPlayerEntity(Player player)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var entity = PhotonNetwork.Instantiate(playerEntity.name, Vector3.zero, Quaternion.identity);
        entity.name = player.NickName;
    }
}
