using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    PlayerControls playerControls;
    [HideInInspector] public PlayerManager player;

    [Header("Player movement")]
    private Vector2 playerMovementInput;
    private float playerVerticalInput, playerHorizontalInput;
    private float playerMoveAmount;

    [Header("Camera movement")]
    private Vector2 cameraMovementInput;
    private float cameraVerticalInput, cameraHorizontalInput;

    [Header("Player action")]
    [SerializeField] private bool dodgeInput = false;

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

        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {

            Instance.enabled = true;
        }
        else
        {
            Instance.enabled = false;
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed += (ctx) => playerMovementInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Look.performed += (ctx) => cameraMovementInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Crouch.performed += (ctx) => dodgeInput = true;

            playerControls.Enable();
        }
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void Update()
    {
        HandleAllInput();

    }
    private void HandleAllInput()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
    }
    private void HandlePlayerMovementInput()
    {


        playerVerticalInput = playerMovementInput.y;
        playerHorizontalInput = playerMovementInput.x;
        playerMoveAmount = Mathf.Clamp01(Mathf.Abs(playerHorizontalInput) + Mathf.Abs(playerVerticalInput));



        if (player.playerAnimator != null)
        {
            player.playerAnimator.UpdateMovementParameters(0, playerMoveAmount);
        }

    }
    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraMovementInput.y;
        cameraHorizontalInput = cameraMovementInput.x;
    }
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            player.playerLocoMotion.AttemptToPerformDodge();
        }
    }

    public float GetPlayerMoveAmount()
    {
        return playerMoveAmount;
    }
    public float GetPlayerVerticalInput()
    {
        return playerVerticalInput;
    }
    public float GetPlayerHorizontalInput()
    {
        return playerHorizontalInput;
    }

    public float GetCameraVerticalInput()
    {
        return cameraVerticalInput;
    }
    public float GetCameraHorizontalInput()
    {
        return cameraHorizontalInput;
    }
}
