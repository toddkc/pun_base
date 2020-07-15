using UnityEngine;
using UnityEngine.UI;

public class ChatManagerUI : MonoBehaviour
{
    [SerializeField] InputField chatMessageInput = default;
    [SerializeField] Text messageDisplayText = default;
    [SerializeField] GameObject chatPanel = default;
    [SerializeField] KeyCode toggleKey = default;
    [SerializeField] TestScripts.ChatManager chat;

    private void Start()
    {
        chatPanel.SetActive(false);
    }

    private void OnEnable()
    {
        chat.OnUpdateMessages += UpdateMessages;
    }

    private void OnDisable()
    {
        chat.OnUpdateMessages -= UpdateMessages;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && chat.InRoom)
        {
            if (chat.ChatActive)
            {
                if (!chatMessageInput.isFocused)
                {
                    chatPanel.SetActive(false);
                    chat.ChatActive = false;
                }
            }
            else
            {
                chatPanel.SetActive(true);
                chat.ChatActive = true;
            }
        }

        if (chat.ChatActive && chat.InRoom)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                SendChatMessage();
            }
        }
    }

    public void EnableChat()
    {
        chatPanel.SetActive(true);
    }

    public void DisableChat()
    {
        chatMessageInput.text = "";
        chatPanel.SetActive(false);
    }

    public void SendChatMessage()
    {
        string message = chatMessageInput.text;
        if (string.IsNullOrEmpty(message)) return;
        chatMessageInput.text = "";
        chatMessageInput.ActivateInputField();
        chat.SendChatMessage(message);
    }

    private void UpdateMessages(string messages)
    {
        messageDisplayText.text = messages;
    }
}
