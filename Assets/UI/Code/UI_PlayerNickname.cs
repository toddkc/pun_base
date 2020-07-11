using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerNickname : MonoBehaviourPun
{
    [SerializeField] InputField nicknameInput = default;
    private const string playerPrefsKey = "nickname";

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            nicknameInput.text = PlayerPrefs.GetString(playerPrefsKey);
        }
        else
        {
            nicknameInput.text = "Player" + Random.Range(111111, 999999);
        }
    }

    private void OnEnable()
    {
        nicknameInput.onValueChanged.AddListener(UpdatePlayerNickname);
    }

    private void UpdatePlayerNickname(string nickname)
    {
        //string nickname = nicknameInput.text;
        PlayerPrefs.SetString(playerPrefsKey, nickname);
        PhotonNetwork.NickName = nickname;
        Debug.Log("player nickname set as: " + nickname);
    }
}
