using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageDisplay : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text messageText;
    [SerializeField] float messageDelay;
    private WaitForSeconds delay;

    private void OnEnable()
    {
        delay = new WaitForSeconds(messageDelay);
    }

    public void DisplayMessage()
    {
        StopAllCoroutines();
        string message = PlayerPrefs.GetString("message");
        if (!string.IsNullOrEmpty(message))
        {
            messageText.text = message;
            panel.SetActive(true);
            StartCoroutine(DelayHideMessage());
        }
    }

    private IEnumerator DelayHideMessage()
    {
        yield return delay;
        messageText.text = "";
        panel.SetActive(false);
    }
}
