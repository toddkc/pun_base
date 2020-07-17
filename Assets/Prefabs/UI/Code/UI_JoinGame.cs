namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_JoinGame : MonoBehaviour
    {
        [SerializeField] InputField roomNameInput = default;

        private void OnEnable()
        {
            roomNameInput.onValueChanged.AddListener(OnInputChanged);
        }
        private void OnDisable()
        {
            roomNameInput.onValueChanged.RemoveListener(OnInputChanged);
        }

        public void OnInputChanged(string text)
        {
            PlayerPrefs.SetString("joinroom", text);
        }
    }
}