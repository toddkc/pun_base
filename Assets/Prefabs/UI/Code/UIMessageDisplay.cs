using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageDisplay : MonoBehaviour
{
    public static UIMessageDisplay instance;
    [SerializeField] GameObject panel;
    [SerializeField] Text messageText;
    [SerializeField] float messageDelay;
    private WaitForSeconds delay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            delay = new WaitForSeconds(messageDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplayMessage(string message)
    {
        StopAllCoroutines();
        messageText.text = message;
        panel.SetActive(true);
        StartCoroutine(DelayHideMessage());
    }

    private IEnumerator DelayHideMessage()
    {
        yield return delay;
        messageText.text = "";
        panel.SetActive(false);
    }
}
