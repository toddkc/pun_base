using Photon.Pun;
using UnityEngine;

public class CustomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerEntity = default;

    // need to disconnect from chat and voice when leaving?

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("player is not connected to PUN");
            return;
        }

        // instantiate player
        var entity = PhotonNetwork.Instantiate(playerEntity.name, Vector3.zero, Quaternion.identity);
        entity.name = PhotonNetwork.NickName;
    }
}
