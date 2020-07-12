using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageDisplay : MonoBehaviour
{
    public static ErrorMessageDisplay instance;
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
