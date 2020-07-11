using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] AudioClip playerJoinedRoom = default;
    [SerializeField] AudioClip playerLeftRoom = default;

    public static AudioPlayer instance;
    private AudioSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        instance.source.PlayOneShot(clip);
    }


    public static void PlayerJoined()
    {
        if (instance.playerJoinedRoom) PlaySound(instance.playerJoinedRoom);
    }

    public static void PlayerLeft()
    {
        if (instance.playerLeftRoom) PlaySound(instance.playerLeftRoom);
    }
}
