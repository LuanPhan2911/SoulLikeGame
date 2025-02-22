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
        playerLocoMotionManager.HandleAllMovement();
    }
}
