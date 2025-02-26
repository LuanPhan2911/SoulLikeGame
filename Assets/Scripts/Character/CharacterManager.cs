using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterNetworkManager characterNetwork;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool applyRootMotion = false;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetwork = GetComponent<CharacterNetworkManager>();


    }

    protected virtual void Update()
    {
        //if (IsOwner)
        //{
        //    characterNetworkManager.SetNetworkPosition(transform.position);
        //    characterNetworkManager.SetNetworkRoation(transform.rotation);


        //}
        //else
        //{
        //    transform.position =
        //        Vector3.SmoothDamp(transform.position,
        //        characterNetworkManager.GetNetworkPosition(),
        //        ref characterNetworkManager.networkPositionVelocity,
        //        characterNetworkManager.networkMovementSmoothTimer
        //       );

        //    transform.rotation = Quaternion.Slerp(transform.rotation,
        //        characterNetworkManager.GetNetworkRotation(),
        //        characterNetworkManager.networkRotationSmoothTimer
        //        );
        //}
    }
    protected virtual void LateUpdate()
    {

    }
}
