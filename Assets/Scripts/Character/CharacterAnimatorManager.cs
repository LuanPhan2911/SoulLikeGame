using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }



    public void UpdateMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }
    public void PlayerTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canMove = false,
        bool canRotate = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;

        character.characterNetwork.PlayActionAnimationServerRPC(
            NetworkManager.Singleton.LocalClientId,
            targetAnimation,
            applyRootMotion
        );
    }
}
