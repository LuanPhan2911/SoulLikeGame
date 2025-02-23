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
}
