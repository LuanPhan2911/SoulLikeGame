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

    }
    protected virtual void LateUpdate()
    {

    }

}
