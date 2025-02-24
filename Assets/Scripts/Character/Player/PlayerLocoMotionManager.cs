using UnityEngine;

public class PlayerLocoMotionManager : CharacterLocoMotionManager
{
    private PlayerManager playerManager;
    public float horizontalMovement, verticalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 targetRotationDirection;

    override protected void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }
    override protected void Update()
    {
        base.Update();
        if (playerManager.IsOwner)
        {
            playerManager.characterNetworkManager.SetNetworkMoveAmount(moveAmount);
            playerManager.characterNetworkManager.SetHorizontalParameter(horizontalMovement);
            playerManager.characterNetworkManager.SetVerticalParameter(verticalMovement);
        }
        else
        {
            moveAmount = playerManager.characterNetworkManager.GetNetworkMoveAmount();
            horizontalMovement = playerManager.characterNetworkManager.GetHorizontalParameter();
            verticalMovement = playerManager.characterNetworkManager.GetVerticalParameter();

            playerManager.playerAnimatorManager.UpdateMovementParameters(0, moveAmount);
        }

    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
    }
    private void GetVerticalAndHorizontalInput()
    {
        verticalMovement = PlayerInputManager.Instance.GetPlayerVerticalInput();
        horizontalMovement = PlayerInputManager.Instance.GetPlayerHorizontalInput();
        moveAmount = PlayerInputManager.Instance.GetPlayerMoveAmount();
    }
    private void HandleGroundMovement()
    {
        GetVerticalAndHorizontalInput();
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.Instance.GetPlayerMoveAmount() > 0.5f)
        {
            //move at running speed
            playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance.GetPlayerMoveAmount() >= 0.5f)
        {
            //move at walking speed
            playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }
    private void HandleRotation()
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
}
