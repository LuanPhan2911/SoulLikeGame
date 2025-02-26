using UnityEngine;

public class PlayerLocoMotionManager : CharacterLocoMotionManager
{
    [HideInInspector] private PlayerManager player;
    [Header("Movement Parameters")]
    [HideInInspector] public float horizontalMovement, verticalMovement;
    [HideInInspector] public float moveAmount;

    private Vector3 moveDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 targetRotationDirection;
    [Header("Dodge Parameters")]
    private Vector3 rollDirection;

    override protected void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    override protected void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetwork.SetNetworkMoveAmount(moveAmount);
            player.characterNetwork.SetHorizontalParameter(horizontalMovement);
            player.characterNetwork.SetVerticalParameter(verticalMovement);
        }
        else
        {
            moveAmount = player.characterNetwork.GetNetworkMoveAmount();
            horizontalMovement = player.characterNetwork.GetHorizontalParameter();
            verticalMovement = player.characterNetwork.GetVerticalParameter();

            player.playerAnimator.UpdateMovementParameters(0, moveAmount);
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
        if (!player.canMove)
        {
            return;
        }
        GetVerticalAndHorizontalInput();

        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.Instance.GetPlayerMoveAmount() > 0.5f)
        {
            //move at running speed
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance.GetPlayerMoveAmount() >= 0.5f)
        {
            //move at walking speed
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }
    private void HandleRotation()
    {
        if (!player.canRotate)
        {
            return;
        }
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

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
        {
            return;
        }
        if (PlayerInputManager.Instance.GetPlayerMoveAmount() > 0)
        {
            rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            rollDirection.Normalize();
            rollDirection.y = 0;

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;
            // perform roll animation

            player.playerAnimator.PlayerTargetActionAnimation("Roll_Forward_01", true, true);
        }
        else
        {
            player.playerAnimator.PlayerTargetActionAnimation("Back_Step_01", true, true);
            // perform backstep animation
        }
    }
}
