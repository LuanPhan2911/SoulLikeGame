using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    PlayerControls playerControls;
    [SerializeField] private Vector2 movement;

    public static PlayerInputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        Debug.Log("PlayerInputManager enabled");
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {

            Instance.enabled = true;
        }
        else
        {
            Instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
            playerControls.Enable();
        }
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
