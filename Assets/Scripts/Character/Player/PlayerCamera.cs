using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public static PlayerCamera Instance { get; private set; }

    public Camera cameraObject;
    public PlayerManager playerManager;
    [SerializeField] private Transform pivotCameraTransform;

    [Header("Camera setting")]

    private float cameraSmoothSpeed = 1f;
    [SerializeField] private float upAndDownRotationSpeed = 10;
    [SerializeField] private float leftAndRightTotationSpeed = 20;
    // the minimum angle the camera can look down
    [SerializeField] private float minPivot = -30;
    // the maximum angle the camera can look up
    [SerializeField] private float maxPivot = 60;
    [SerializeField] private float cameraCollisionRadius = 0.2f;

    [SerializeField] private LayerMask collisionWithLayer;

    [Header("Camera values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPostion;
    private float upAndDownLookAngle;
    private float leftAndRightLookAngle;
    private float defaultZCameraPosition;
    private float targetZCameraPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        defaultZCameraPosition = cameraObject.transform.position.z;
    }

    public void HandleAllCameraActions()
    {
        if (playerManager != null)
        {
            // follow player
            HandleFollowTarget();

            // rotate camera
            HandleRotation();
            // collide with objects
            HandleCollision();
        }

    }
    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position,
            ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotation()
    {
        // if looked target on, rotate around target

        //else rotate regularly

        leftAndRightLookAngle += (PlayerInputManager.Instance.GetCameraHorizontalInput() * leftAndRightTotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.Instance.GetCameraVerticalInput() * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);

        //rotate transform left and right


        transform.rotation = Quaternion.Euler(new Vector3(0f, leftAndRightLookAngle, 0));

        //rotate pivot camera trasnform up and down

        pivotCameraTransform.localRotation = Quaternion.Euler(new Vector3(upAndDownLookAngle, 0f, 0f));



    }
    private void HandleCollision()
    {
        targetZCameraPosition = defaultZCameraPosition;


        Vector3 direction = cameraObject.transform.position - pivotCameraTransform.position;
        direction.Normalize();
        if (Physics.SphereCast(pivotCameraTransform.position, cameraCollisionRadius, direction, out RaycastHit hit,
            Mathf.Abs(targetZCameraPosition), collisionWithLayer))
        {
            float distance = Vector3.Distance(pivotCameraTransform.position, hit.point);
            targetZCameraPosition = -(distance - cameraCollisionRadius);
        }
        if (Mathf.Abs(targetZCameraPosition) < cameraCollisionRadius)
        {
            targetZCameraPosition = -cameraCollisionRadius;
        }
        cameraObjectPostion.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetZCameraPosition, 0.1f);
        cameraObject.transform.localPosition = cameraObjectPostion;
    }
}
