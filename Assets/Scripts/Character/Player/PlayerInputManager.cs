using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    PlayerControls playerControls;
    private Vector2 movementInput;
    private float verticalInput, horizontalInput;
    private float moveAmount;

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
            playerControls.Player.Move.performed += (ctx) => movementInput = ctx.ReadValue<Vector2>();

            playerControls.Enable();
        }
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void Update()
    {

        HandleMovementInput();

    }
    private void HandleMovementInput()
    {


        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (moveAmount <= 0.5f)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f)
        {
            moveAmount = 1f;
        }
    }

    public float GetMoveAmount()
    {
        return moveAmount;
    }
    public float GetVerticalInput()
    {
        return verticalInput;
    }
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
}
