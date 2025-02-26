using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{

    [Header("Position")]
    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
     NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositionVelocity;
    public float networkMovementSmoothTimer = 0.1f;
    public float networkRotationSmoothTimer = 0.1f;

    [Header("Animator Parameters")]
    private NetworkVariable<float> animatorVeticalParameter = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> animatorHorizontalParameter = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> networkMoveAmount = new NetworkVariable<float>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private CharacterManager characterManager;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }


    [ServerRpc]
    public void PlayActionAnimationServerRPC(ulong clientId, string animationId, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayActionAnimationClientRPC(clientId, animationId, applyRootMotion);
        }
    }
    [ClientRpc]
    private void PlayActionAnimationClientRPC(ulong clientId, string animationId, bool applyRootMotion)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            characterManager.animator.CrossFade(animationId, 0.2f);
            characterManager.applyRootMotion = applyRootMotion;
        }
    }

    public void SetNetworkPosition(Vector3 position)
    {
        networkPosition.Value = position;
    }
    public void SetNetworkRoation(Quaternion rotation)
    {
        networkRotation.Value = rotation;
    }

    public void SetNetworkMoveAmount(float moveAmount)
    {
        networkMoveAmount.Value = moveAmount;
    }
    public void SetHorizontalParameter(float horizontal)
    {
        animatorHorizontalParameter.Value = horizontal;
    }
    public void SetVerticalParameter(float vertical)
    {
        animatorVeticalParameter.Value = vertical;
    }
    public Vector3 GetNetworkPosition()
    {
        return networkPosition.Value;
    }
    public Quaternion GetNetworkRotation()
    {
        return networkRotation.Value;
    }
    public float GetNetworkMoveAmount()
    {
        return networkMoveAmount.Value;
    }
    public float GetHorizontalParameter()
    {
        return animatorHorizontalParameter.Value;
    }
    public float GetVerticalParameter()
    {
        return animatorVeticalParameter.Value;
    }

}
