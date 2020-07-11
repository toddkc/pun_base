using UnityEngine;

public class UI_TogglePanelWithKey : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] KeyCode onkey;
    [SerializeField] KeyCode offkey;

    private void Update()
    {
        if(Input.GetKeyDown(onkey) && !panel.activeSelf)
        {
            panel.SetActive(true);
        }

        if(Input.GetKeyDown(offkey) && panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }
}
