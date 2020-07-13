using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    // TODO:
    // separate manager from ui

    [SerializeField] bool printDebug = false;
    [SerializeField] InputField chatMessageInput = default;
    [SerializeField] Text messageDisplayText = default;
    [SerializeField] GameObject chatPanel = default;
    [SerializeField] KeyCode onkey = default;
    [SerializeField] KeyCode offkey = default;

    private ChatClient client;
    private string[] channelRoomNames = new string[1];
    private bool chatActive = false;
    private bool inRoom = false;
    private ChatAppSettings appSettings;

    private void Start()
    {
        chatPanel.SetActive(false);
    }

    public void EnableChat()
    {
        channelRoomNames[0] = PhotonNetwork.CurrentRoom.Name;
        appSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        AuthenticationValues authvals = new AuthenticationValues();
        authvals.UserId = PhotonNetwork.NickName;
        authvals.AuthType = CustomAuthenticationType.None;
        client = new ChatClient(this);
        client.AuthValues = authvals;
        client.ChatRegion = "US";
        client.UseBackgroundWorkerForSending = true;
        if (printDebug) Debug.Log("connecting to chat as: " + client.AuthValues.UserId);
        client.ConnectUsingSettings(appSettings);
        inRoom = true;
        chatActive = true;
        chatPanel.SetActive(true);
    }

    public void DisableChat()
    {
        client.Unsubscribe(channelRoomNames);
        inRoom = false;
        chatActive = false;
        chatPanel.SetActive(false);
        if (printDebug) Debug.Log("unsubscribing from chat");
    }

    public void SendChatMessage()
    {
        string message = chatMessageInput.text;
        if (string.IsNullOrEmpty(message)) return;
        chatMessageInput.text = "";
        chatMessageInput.ActivateInputField();
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, message);
    }

    private void UpdateMessages(string channelName)
    {
        ChatChannel channel = null;
        bool found = client.TryGetChannel(channelName, out channel);
        if (!found)
        {
            if (printDebug) Debug.Log("failed to find channel: " + channelName);
            return;
        }

        messageDisplayText.text = channel.ToStringMessages();
    }

    private void Update()
    {
        if (Input.GetKeyDown(onkey) && inRoom)
        {
            if (chatActive)
            {
                if (!chatMessageInput.isFocused)
                {
                    chatPanel.SetActive(false);
                    chatActive = false;
                }
            }
            else
            {
                chatPanel.SetActive(true);
                chatActive = true;
            }
        }

        if (chatActive && inRoom)
        {
            client.Service();
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                SendChatMessage();
            }
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (printDebug) Debug.Log("chat return");
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        client.Subscribe(channelRoomNames);
        if (printDebug) Debug.Log("connected to photon chat: " + client.ChatRegion);
    }

    public void OnDisconnected()
    {
        if (printDebug) Debug.Log("disconnected from photon chat");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (printDebug) Debug.Log("chat message received");
        UpdateMessages(channelName);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (printDebug) Debug.Log("private chat message received");
        UpdateMessages(channelName);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        if (printDebug) Debug.Log("status updated");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        if (printDebug) Debug.Log("photon chat subscribed: " + channels[0] + ", channels: " + channels.Length);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has joined");
    }

    public void OnUnsubscribed(string[] channels)
    {
        if (printDebug) Debug.Log("photon chat unsubscribed, channels: " + channels.Length);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has left");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        if (printDebug) Debug.Log(user + " joined " + channel);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, user + " has joined " + channel);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        if (printDebug) Debug.Log(user + " left " + channel);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, user + " has left " + channel);
    }
}
