using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.UI;

public class VoiceDebugUI : MonoBehaviour
{
    PhotonVoiceView view;
    [SerializeField] Image micImage;
    [SerializeField] Image speakerImage;

    private void Start()
    {
        view = GetComponent<PhotonVoiceView>();
    }

    private void Update()
    {
        micImage.enabled = view.IsRecording;
        speakerImage.enabled = view.IsSpeaking;
    }
}
