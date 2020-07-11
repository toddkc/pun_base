using UnityEngine;

public class UI_RandomGame : MonoBehaviour
{
    public void RandomGame()
    {
        CustomNetworkManager.instance.CreateOrJoinRandomRoom();
    }
}
