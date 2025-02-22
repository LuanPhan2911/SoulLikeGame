using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }
    [Header("Newwork join")]
    [SerializeField] private bool startGameAsClient;

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
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //we must shutdown the server because we have started as host on the title scene
            NetworkManager.Singleton.Shutdown();

            NetworkManager.Singleton.StartClient();
        }
    }
}
