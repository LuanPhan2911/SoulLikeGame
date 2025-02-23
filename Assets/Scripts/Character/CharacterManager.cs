using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    public CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();

    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            characterNetworkManager.SetNetworkPosition(transform.position);
            characterNetworkManager.SetNetworkRoation(transform.rotation);
        }
        else
        {
            transform.position =
                Vector3.SmoothDamp(transform.position,
                characterNetworkManager.GetNetworkPosition(),
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkMovementSmoothTimer
               );

            transform.rotation = Quaternion.Slerp(transform.rotation,
                characterNetworkManager.GetNetworkRotation(),
                characterNetworkManager.networkRotationSmoothTimer
                );
        }
    }
    protected virtual void LateUpdate()
    {

    }
}
