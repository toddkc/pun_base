namespace NetworkTutorial.TicTacToe
{
    using Photon.Pun;
    using Photon.Realtime;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class TicTacToeGame : MonoBehaviour
    {
        [SerializeField] Space[] spaces = new Space[9];
        [SerializeField] GameObject[] turnDisplay = new GameObject[2];
        [SerializeField] Text gameOverText = default;
        [SerializeField] GameObject gameOverPanel = default;
        [SerializeField] GameObject playerPrefab = default;

        public readonly string[] sides = new string[2] { "X", "O" };
        private int moves;
        private bool isPlaying = false;
        public int activePlayer { get; set; }
        private int localPlayer;
        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        private TicTacToePlayer view;

        public void Start()
        {
            // setup both players info
            activePlayer = 0;
            if (PhotonNetwork.IsMasterClient)
            {
                localPlayer = 0;
                players.Add(0, PhotonNetwork.LocalPlayer);
                if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
                {
                    Debug.Log("only two players can play tic tac toe");
                }
                foreach (var player in PhotonNetwork.CurrentRoom.Players)
                {
                    if (player.Value != PhotonNetwork.LocalPlayer)
                    {
                        players.Add(1, player.Value);
                    }
                }
            }
            else
            {
                localPlayer = 1;
                players.Add(1, PhotonNetwork.LocalPlayer);
                foreach (var player in PhotonNetwork.CurrentRoom.Players)
                {
                    if (player.Value != PhotonNetwork.LocalPlayer)
                    {
                        players.Add(0, player.Value);
                    }
                }
            }

            // spawn player with photon view
            GameObject myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            myPlayer.name = PhotonNetwork.NickName;
            view = myPlayer.GetComponent<TicTacToePlayer>();
            view.PlayerIndex = localPlayer;

            // setup game
            SetGameControllerInSpaces();
            OnResetGame();
            HostStartGame();
        }

        // host will send event to start
        private void HostStartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PUN_Events.StartGameEvent();
        }

        // host will send event to reset
        public void HostResetGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PUN_Events.ResetGameEvent();
        }

        // toggle bool of game state
        public void OnStartGame()
        {
            isPlaying = true;
            turnDisplay[0].SetActive(true);
        }

        // resets the board and score
        public void OnResetGame()
        {
            activePlayer = 0;
            moves = 0;
            turnDisplay[1].SetActive(false);
            turnDisplay[0].SetActive(true);
            gameOverPanel.SetActive(false);
            ResetSpaces();
        }

        // set the spaces ref to this controller
        private void SetGameControllerInSpaces()
        {
            for (int i = 0; i < spaces.Length; i++)
            {
                spaces[i].GameController = this;
                spaces[i].SpaceIndex = i;
            }
        }

        // clear all spaces text
        private void ResetSpaces()
        {
            foreach (var space in spaces)
            {
                space.ResetSpace();
            }
        }

        // set the text of a space and end the turn
        public void SetSpace(int spaceIndex, int playerIndex)
        {
            if (playerIndex != activePlayer) return;
            spaces[spaceIndex].SetSpace(sides[playerIndex]);
            EndTurn();
        }

        // when a player clicks a space button
        public void AttemptTurn(Space space)
        {
            string debug = string.Format("attempt turn local -- isplaying: {0}, activePlayer: {1}, localPlayer: {2}", isPlaying, activePlayer, localPlayer);
            Debug.Log(debug);
            if (!isPlaying || activePlayer != localPlayer) return;
            view.AttemptTurn(space.SpaceIndex);
        }

        // check the game state and activate next player
        private void EndTurn()
        {
            moves++;
            CheckBoard();
            turnDisplay[activePlayer].SetActive(false);
            activePlayer = activePlayer == 0 ? 1 : 0;
            turnDisplay[activePlayer].SetActive(true);
        }

        // check win or draw
        private void CheckBoard()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (spaces[0].GetText == sides[activePlayer] && spaces[1].GetText == sides[activePlayer] && spaces[2].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[3].GetText == sides[activePlayer] && spaces[4].GetText == sides[activePlayer] && spaces[5].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[6].GetText == sides[activePlayer] && spaces[7].GetText == sides[activePlayer] && spaces[8].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[0].GetText == sides[activePlayer] && spaces[3].GetText == sides[activePlayer] && spaces[6].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[1].GetText == sides[activePlayer] && spaces[4].GetText == sides[activePlayer] && spaces[7].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[2].GetText == sides[activePlayer] && spaces[5].GetText == sides[activePlayer] && spaces[8].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[0].GetText == sides[activePlayer] && spaces[4].GetText == sides[activePlayer] && spaces[8].GetText == sides[activePlayer])
                GameOver(activePlayer);
            else if (spaces[2].GetText == sides[activePlayer] && spaces[4].GetText == sides[activePlayer] && spaces[6].GetText == sides[activePlayer])
                GameOver(activePlayer);

            if (moves >= 9)
            {
                GameOver(-1);
            }
        }

        // called by host to signal game over
        private void GameOver(int winner)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            view.GameOver(winner);
        }

        // called from player photon view on server rpc game over
        public void OnGameOver(int winningPlayer)
        {
            turnDisplay[0].SetActive(false);
            turnDisplay[1].SetActive(false);
            switch (winningPlayer)
            {
                case -1:
                    gameOverText.text = "Tie!";
                    break;
                default:
                    gameOverText.text = players[winningPlayer].NickName + " won!";
                    break;
            }
            gameOverPanel.SetActive(true);
        }
    }
}