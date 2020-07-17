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
            localPlayer = 1;
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
            PUN_Events.StartGameEvent();
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

        public void SetSpace(int spaceIndex, int playerIndex)
        {
            if (playerIndex != activePlayer) return;
            // if this ever gets out of sync this method is called from master
            // activePlayer = playerIndex;
            spaces[spaceIndex].SetSpace(sides[playerIndex]);
            EndTurn();
        }

        public void AttemptTurn(Space space)
        {
            string debug = string.Format("attempt turn local -- isplaying: {0}, activePlayer: {1}, localPlayer: {2}", isPlaying, activePlayer, localPlayer);
            Debug.Log(debug);
            if (!isPlaying || activePlayer != localPlayer) return;
            view.AttemptTurn(space.SpaceIndex);
        }

        private void EndTurn()
        {
            Debug.Log("end turn");
            turnDisplay[activePlayer].SetActive(false);
            activePlayer = activePlayer == 0 ? 1 : 0;
            turnDisplay[activePlayer].SetActive(true);
        }
    }
}