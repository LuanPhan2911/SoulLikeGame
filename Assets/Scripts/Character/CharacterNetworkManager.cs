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

    public void SetNetworkPosition(Vector3 position)
    {
        networkPosition.Value = position;
    }
    public void SetNetworkRoation(Quaternion rotation)
    {
        networkRotation.Value = rotation;
    }

    public Vector3 GetNetworkPosition()
    {
        return networkPosition.Value;
    }
    public Quaternion GetNetworkRotation()
    {
        return networkRotation.Value;
    }

}
