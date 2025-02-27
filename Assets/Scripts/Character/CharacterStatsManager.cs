using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{

    [Header("Stamina Stat")]
    private float staminaRegenerateTimer = 0;
    [SerializeField] private float staminaRegenerateDelay = 2f;
    [SerializeField] private float staminaStickTimer = 0f;
    [SerializeField] private int staminaRegenerateAmount = 2;


    private CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public int CalculateStaminaBaseOnEnduranceLevel(int endurance)
    {
        float stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenrateStamina()
    {
        if (!character.IsOwner)
        {
            return;
        }
        if (character.characterNetwork.GetIsSprinting())
        {
            return;
        }
        if (character.isPerformingAction)
        {
            return;
        }
        staminaRegenerateTimer += Time.deltaTime;
        if (staminaRegenerateTimer > staminaRegenerateDelay)
        {
            if (character.characterNetwork.currentStamina.Value < character.characterNetwork.maxStamina.Value)
            {
                staminaStickTimer += Time.deltaTime;
                if (staminaStickTimer > 0.1f)
                {
                    staminaStickTimer = 0;
                    character.characterNetwork.currentStamina.Value += staminaRegenerateAmount;
                }
            }
        }
    }
    public virtual void ResetStaminaRegenerateTimer(float oldVlaue, float newValue)
    {
        if (newValue < oldVlaue)
        {
            staminaRegenerateTimer = 0;
        }

    }


}
