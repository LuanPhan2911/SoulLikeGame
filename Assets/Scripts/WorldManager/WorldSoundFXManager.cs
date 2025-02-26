using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance { get; private set; }

    [Header("Sound FX")]
    public AudioClip rollClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
