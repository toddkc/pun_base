using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts
{
    public class ChatManager : MonoBehaviour, IChatClientListener
    {
        [SerializeField] bool printDebug = false;
        private ChatClient client;
        private string[] channelRoomNames = new string[1];
        public bool ChatActive { get; set; }
        public bool InRoom { get; private set; }
        private ChatAppSettings appSettings;
        public event Action<string> OnUpdateMessages;

        private void Start()
        {
            ChatActive = false;
            InRoom = false;
        }

        private void Update()
        {
            if (ChatActive && InRoom)
            {
                client.Service();
            }
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
            InRoom = true;
            ChatActive = true;
        }

        public void DisableChat()
        {
            client.Unsubscribe(channelRoomNames);
            InRoom = false;
            ChatActive = false;
            if (printDebug) Debug.Log("unsubscribing from chat");
        }

        public void SendChatMessage(string message)
        {
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

            OnUpdateMessages(channel.ToStringMessages());
        }

        public void OnConnected()
        {
            client.Subscribe(channelRoomNames);
            if (printDebug) Debug.Log("connected to photon chat: " + client.ChatRegion);
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

        public void OnUnsubscribed(string[] channels)
        {
            if (printDebug) Debug.Log("photon chat unsubscribed, channels: " + channels.Length);
            client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has left");
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            if (printDebug) Debug.Log("photon chat subscribed: " + channels[0] + ", channels: " + channels.Length);
            client.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + " has joined");
        }

        public void OnDisconnected()
        {
            if (printDebug) Debug.Log("disconnected from photon chat");
        }
        public void DebugReturn(DebugLevel level, string message)
        {
            if (printDebug) Debug.Log("chat return");
        }
        public void OnChatStateChange(ChatState state)
        {
        }
    }
}