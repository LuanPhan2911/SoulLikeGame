using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager characterManager;

    protected void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }



    public void UpdateMovementParameters(float horizontalValue, float verticalValue)
    {
        characterManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        characterManager.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }
}
