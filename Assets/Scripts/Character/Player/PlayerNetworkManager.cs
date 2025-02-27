using Unity.Collections;
using Unity.Netcode;

public class PlayerNetworkManager : CharacterNetworkManager
{
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>(new FixedString64Bytes("Player"),
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
}
