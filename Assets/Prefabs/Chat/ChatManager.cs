using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    [Header("UI References")]
    [SerializeField] InputField chatMessageInput = default;
    [SerializeField] Text messageDisplayText = default;

    [Header("Settings")]
    [SerializeField] private float chatUpdateDelay = 1.0f;

    private ChatClient client;
    private bool chatActive = false;
    private WaitForSeconds delay;
    private ChatAppSettings appSettings;

    private void Start()
    {
        delay = new WaitForSeconds(chatUpdateDelay);
        appSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        AuthenticationValues authvals = new AuthenticationValues();
        authvals.UserId = PhotonNetwork.NickName;
        authvals.AuthType = CustomAuthenticationType.None;
        client = new ChatClient(this);
        client.AuthValues = authvals;
        client.ChatRegion = "US";
        if (string.IsNullOrEmpty(appSettings.AppId))
        {
            Debug.Log("no app id set for chat");
        }
        client.UseBackgroundWorkerForSending = true;
        Debug.Log("connecting to chat as: " + client.AuthValues.UserId);
        client.ConnectUsingSettings(appSettings);
    }

    public void SendChatMessage()
    {
        string message = chatMessageInput.text;
        if (string.IsNullOrEmpty(message)) return;
        chatMessageInput.text = "";
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, message);
    }

    public void UpdateMessages(string channelName)
    {
        ChatChannel channel = null;
        bool found = client.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("failed to find channel: " + channelName);
            return;
        }

        messageDisplayText.text = channel.ToStringMessages();
    }

    private void Update()
    {
        client.Service();
    }

    public void ToggleChat()
    {
        if (chatActive)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(UpdateChat());
        }
        chatActive = !chatActive;
    }

    private IEnumerator UpdateChat()
    {
        yield return delay;
        if (client != null)
        {
            client.Service();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("chat return");
    }

    public void OnChatStateChange(ChatState state)
    {
        // do nothing
        return;
    }

    public void OnConnected()
    {
        client.Subscribe(PhotonNetwork.CurrentRoom.Name);
        Debug.Log("connected to photon chat: " + client.ChatRegion);
    }

    public void OnDisconnected()
    {
        Debug.Log("disconnected from photon chat");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("chat message received");
        UpdateMessages(channelName);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("private chat message received");
        UpdateMessages(channelName);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status updated");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("photon chat subscribed: " + channels[0] + ", channels: " + channels.Length);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has joined");
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("photon chat unsubscribed, channels: " + channels.Length);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has left");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log(user + " joined " + channel);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, user + " has joined " + channel);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log(user + " left " + channel);
        client.PublishMessage(PhotonNetwork.CurrentRoom.Name, user + " has left " + channel);
    }
}
