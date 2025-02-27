using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocoMotionManager playerLocoMotion;
    [HideInInspector] public PlayerAnimatorManager playerAnimator;
    [HideInInspector] public PlayerNetworkManager playerNetwork;
    [HideInInspector] public PlayerStatsManager playerStats;
    protected override void Awake()
    {
        base.Awake();
        playerLocoMotion = GetComponent<PlayerLocoMotionManager>();
        playerAnimator = GetComponent<PlayerAnimatorManager>();
        playerNetwork = GetComponent<PlayerNetworkManager>();
        playerStats = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!IsOwner)
        {
            return;
        }
        playerLocoMotion.HandleAllMovement();
        playerStats.RegenrateStamina();
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

            playerNetwork.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHud.SetNewStaminaValue;
            playerNetwork.currentStamina.OnValueChanged += playerStats.ResetStaminaRegenerateTimer;

            playerNetwork.maxStamina.Value = playerStats.CalculateStaminaBaseOnEnduranceLevel(playerNetwork.endurance.Value);
            playerNetwork.currentStamina.Value = playerStats.CalculateStaminaBaseOnEnduranceLevel(playerNetwork.endurance.Value);
            PlayerUIManager.Instance.playerUIHud.SetMaxStaminaValue(playerNetwork.maxStamina.Value);
        }
    }

    public void SaveGameFromCurrentCharacterData(ref CharacterSaveData characterSaveData)
    {
        characterSaveData.characterName = playerNetwork.characterName.Value.ToString();

        characterSaveData.xPosition = transform.position.x;
        characterSaveData.yPosition = transform.position.y;
        characterSaveData.zPosition = transform.position.z;
    }
    public void LoadGameFromCurrentCharacterData(ref CharacterSaveData characterSaveData)
    {
        playerNetwork.characterName.Value = characterSaveData.characterName;

        transform.position = new Vector3(characterSaveData.xPosition, characterSaveData.yPosition, characterSaveData.zPosition);
    }
}
