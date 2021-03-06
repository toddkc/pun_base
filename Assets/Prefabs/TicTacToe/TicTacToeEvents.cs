﻿namespace NetworkTutorial.TicTacToe
{
    using ExitGames.Client.Photon;
    using Photon.Pun;
    using UnityEngine;

    public class TicTacToeEvents : MonoBehaviour
    {
        [SerializeField] TicTacToeGame controller;

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        private void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == PUN_Events.StartGameEventCode)
            {
                controller.OnStartGame();
            }
            else if (eventCode == PUN_Events.ResetGameEventCode)
            {
                controller.OnResetGame();
            }
        }
    }
}