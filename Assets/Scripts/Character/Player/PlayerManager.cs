using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocoMotionManager playerLocoMotion;
    [HideInInspector] public PlayerAnimatorManager playerAnimator;
    [HideInInspector] public PlayerNetworkManager playerNetwork;
    protected override void Awake()
    {
        base.Awake();
        playerLocoMotion = GetComponent<PlayerLocoMotionManager>();
        playerAnimator = GetComponent<PlayerAnimatorManager>();
        playerNetwork = GetComponent<PlayerNetworkManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!IsOwner)
        {
            return;
        }
        playerLocoMotion.HandleAllMovement();
    }
    protected override void LateUpdate()
    {

        if (!IsOwner)
        {
            return;
        }
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
        }
    }
}
