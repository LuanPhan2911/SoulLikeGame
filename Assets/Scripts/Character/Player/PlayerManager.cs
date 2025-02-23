public class PlayerManager : CharacterManager
{
    PlayerLocoMotionManager playerLocoMotionManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocoMotionManager = GetComponent<PlayerLocoMotionManager>();
    }

    override protected void Update()
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
        }
    }
}
