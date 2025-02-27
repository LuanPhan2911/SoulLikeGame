using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private UIStatBar staminaStatBar;

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaStatBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetMaxStaminaValue(int maxValue)
    {
        staminaStatBar.SetMaxStat(maxValue);
    }
}
