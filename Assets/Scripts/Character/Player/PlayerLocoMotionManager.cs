using UnityEngine;

public class PlayerLocoMotionManager : CharacterLocoMotionManager
{
    [HideInInspector] private PlayerManager player;
    [Header("Movement Parameters")]
    [HideInInspector] public float horizontalMovement, verticalMovement;
    [HideInInspector] public float moveAmount;

    private Vector3 moveDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 4f;
    [SerializeField] private float spritingSpeed = 6.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float sprintingStaminaCost = 2;

    private Vector3 targetRotationDirection;
    [Header("Dodge Parameters")]
    private Vector3 rollDirection;
    [SerializeField] private float dodgeStaminaCost = 25f;

    [Header("Animation")]
    private const string ROLL_FORWARD_01 = "Roll_Forward_01";
    private const string BACK_STEP_01 = "Back_Step_01";


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

            player.playerAnimator.UpdateMovementParameters(0, moveAmount,
                player.playerNetwork.GetIsSprinting(),
                player.playerNetwork.GetIsWalking());
        }

    }

    public void HandleAllMovement()
    {

        HandleGroundedMovement();
        HandleRotation();
    }
    private void GetVerticalAndHorizontalInput()
    {
        verticalMovement = PlayerInputManager.Instance.GetPlayerVerticalInput();
        horizontalMovement = PlayerInputManager.Instance.GetPlayerHorizontalInput();
        moveAmount = PlayerInputManager.Instance.GetPlayerMoveAmount();
    }
    private void HandleGroundedMovement()
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

        if (player.characterNetwork.GetIsSprinting())
        {
            player.characterController.Move(moveDirection * spritingSpeed * Time.deltaTime);
        }
        else
        {
            if (player.characterNetwork.GetIsWalking())
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
            else if (moveAmount >= 2f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
        }

        player.playerAnimator.UpdateMovementParameters(0, moveAmount,
            player.playerNetwork.GetIsSprinting(),
            player.playerNetwork.GetIsWalking());



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
        if (player.characterNetwork.currentStamina.Value <= 0)
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

            player.playerAnimator.PlayerTargetActionAnimation(ROLL_FORWARD_01, true, true);
        }
        else
        {
            player.playerAnimator.PlayerTargetActionAnimation(BACK_STEP_01, true, true);
            // perform backstep animation
        }
        player.characterNetwork.currentStamina.Value -= dodgeStaminaCost;
    }
    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            // set spriting to false
            player.characterNetwork.SetIsSprinting(false);
        }


        // if out of stamina, set sprinting to false
        if (player.characterNetwork.currentStamina.Value <= 0)
        {
            player.characterNetwork.SetIsSprinting(false);
            return;
        }

        //if is moving set sprinting to true
        if (moveAmount >= 1f)
        {
            player.characterNetwork.SetIsSprinting(true);
        }
        else
        {
            player.characterNetwork.SetIsSprinting(false);
        }

        // if is stationary set sprinting to false
        if (player.playerNetwork.GetIsSprinting())
        {
            player.characterNetwork.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }

    }

}
