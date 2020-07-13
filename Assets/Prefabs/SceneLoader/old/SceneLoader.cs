using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // TODO:
    // separate manager from ui

    [SerializeField] private GameObject loadScenePanel = default;
    [SerializeField] private GameObject changeGamePanel = default;
    [SerializeField] private GameEvent sceneLoadedEvent = default;
    [SerializeField] private GameEvent sceneUnloadedEvent = default;

    private bool isSceneLoaded = false;
    private int? currentGameScene = null;

    private void Start()
    {
        loadScenePanel.SetActive(false);
        changeGamePanel.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoadedCallback;
        SceneManager.sceneUnloaded += SceneUnloadedCallback;
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedCallback;
        SceneManager.sceneUnloaded -= SceneUnloadedCallback;
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("join room");
            loadScenePanel.SetActive(true);
            changeGamePanel.SetActive(false);
        }
    }
    public void LeaveRoom()
    {
        Debug.Log("leave room");
        loadScenePanel.SetActive(false);
        changeGamePanel.SetActive(false);
        UnloadGameSceneEventResponse();
    }
    public void JoinGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("join game");
            loadScenePanel.SetActive(false);
            changeGamePanel.SetActive(false);
        }
    }
    public void LeaveGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
        {
            Debug.Log("leave game");
            loadScenePanel.SetActive(true);
            changeGamePanel.SetActive(false);
        }
    }

    // called when unity loads a scene
    // raises unity event and updates player props
    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == currentGameScene)
        {
            sceneLoadedEvent.Raise();
            CustomPlayerProperties.UpdateProps<int>(PhotonNetwork.LocalPlayer, "loaded_scene", scene.buildIndex);
        }
    }

    // called when unity unloads a scene
    // raises unity event
    private void SceneUnloadedCallback(Scene scene)
    {
        if (scene.buildIndex == currentGameScene)
        {
            sceneUnloadedEvent.Raise();
            isSceneLoaded = false;
            currentGameScene = null;
        }
    }

    // button press to load game
    public void LoadGameScene(int scene)
    {
        if (!isSceneLoaded && PhotonNetwork.IsMasterClient)
        {
            PUN_Events.LoadLevelEvent(scene);
        }
    }

    // button press to change game
    public void ChangeGame()
    {
        if (isSceneLoaded && PhotonNetwork.IsMasterClient)
        {
            PUN_Events.UnloadLevelEvent();
        }
    }

    // responds to photon events
    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PUN_Events.LoadLevelEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int buildIndex = (int)data[0];
            LoadGameSceneEventResponse(buildIndex);
        }else if(eventCode == PUN_Events.UnloadLevelEventCode)
        {
            UnloadGameSceneEventResponse();
        }
    }

    // load game scene on pun event
    private void LoadGameSceneEventResponse(int index)
    {
        if (!isSceneLoaded)
        {
            isSceneLoaded = true;
            currentGameScene = index;
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            ErrorMessageDisplay.instance.DisplayMessage("Loading Game...");
        }
    }

    // unload game scene on pun event
    private void UnloadGameSceneEventResponse()
    {
        if (isSceneLoaded && currentGameScene != null)
        {
            SceneManager.UnloadSceneAsync((int)currentGameScene);
        }
    }
}
