using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocoMotionManager playerLocoMotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocoMotionManager = GetComponent<PlayerLocoMotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!IsOwner)
        {
            return;
        }
        playerLocoMotionManager.HandleAllMovement();
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
            PlayerCamera.Instance.playerManager = this;
            PlayerInputManager.Instance.playerManager = this;
        }
    }
}
