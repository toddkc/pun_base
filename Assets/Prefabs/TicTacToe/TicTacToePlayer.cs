﻿namespace NetworkTutorial.TicTacToe
{
    using Photon.Pun;
    using UnityEngine;

    public class TicTacToePlayer : MonoBehaviour
    {
        private PhotonView view;
        public TicTacToeGame GameController { get; set; }
        public int PlayerIndex { get; set; }

        private void Awake()
        {
            view = GetComponent<PhotonView>();
        }

        private void Start()
        {
            GameController = FindObjectOfType<TicTacToeGame>();
        }

        public void OnChangeScene()
        {
            Destroy(gameObject);
        }

        public void AttemptTurn(int spaceIndex)
        {
            view.RPC("RPC_AttemptTurn", RpcTarget.MasterClient, spaceIndex, PlayerIndex);
        }

        public void GameOver(int winningplayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            view.RPC("RPC_GameOver", RpcTarget.All, winningplayer);
        }

        [PunRPC]
        public void RPC_GameOver(int winningplayer)
        {
            GameController.OnGameOver(winningplayer);
        }

        [PunRPC]
        public void RPC_AttemptTurn(int spaceindex, int player)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (player != GameController.activePlayer) return;
            view.RPC("RPC_SetSpace", RpcTarget.All, spaceindex, player);
        }

        [PunRPC]
        public void RPC_SetSpace(int spaceindex, int playerindex)
        {
            GameController.SetSpace(spaceindex, playerindex);
        }
    }
}