using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = default;
    [SerializeField] private GameObject loadScenePanel = default;
    [SerializeField] private GameEvent sceneLoadedEvent = default;
    [SerializeField] private GameEvent sceneUnloadedEvent = default;

    private bool isSceneLoaded = false;

    private void Start()
    {
        loadScenePanel.SetActive(false);
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

    public void ShowGamePanel(bool active)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            loadScenePanel.SetActive(active);
        }
    }

    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == sceneToLoad)
        {
            sceneLoadedEvent.Raise();
            CustomPlayerProperties.UpdateProps<int>(PhotonNetwork.LocalPlayer, "loaded_scene", scene.buildIndex);
        }
    }

    private void SceneUnloadedCallback(Scene scene)
    {
        if (scene.buildIndex == sceneToLoad)
        {
            sceneUnloadedEvent.Raise();
            isSceneLoaded = false;
        }
    }

    public void LoadGameScene(int scene)
    {
        if (!isSceneLoaded && PhotonNetwork.IsMasterClient)
        {
            PUN_Events.LoadLevelEvent(scene);
        }
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PUN_Events.LoadLevelEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int buildIndex = (int)data[0];
            LoadGameSceneEventResponse(buildIndex);
        }
    }

    private void LoadGameSceneEventResponse(int index)
    {
        if (!isSceneLoaded)
        {
            isSceneLoaded = true;
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }
    }

    public void UnloadGameScene()
    {
        if (isSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneToLoad);
        }
    }
}
