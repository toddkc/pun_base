namespace NetworkTutorial.TicTacToe
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Space : MonoBehaviour
    {
        private Button button;
        private Text buttonText;
        public int SpaceIndex { get; set; }
        public TicTacToeGame GameController { get; set; }

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonText = GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(AttemptToTakeSpace);
        }
        private void OnDisable()
        {
            button.onClick.RemoveListener(AttemptToTakeSpace);
        }

        // player is attempting to take space
        public void AttemptToTakeSpace()
        {
            GameController.AttemptTurn(this);
        }

        // reset button state
        public void ResetSpace()
        {
            buttonText.text = "";
            button.interactable = true;
        }

        // set the text on the space
        public void SetSpace(string text)
        {
            buttonText.text = text;
            button.interactable = false;
        }
    }
}