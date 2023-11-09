using UnityEngine;

public class SimplePlayerMovements : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraRoot;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sprintSpeed;

    private InputsActionAsset controls;

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void Awake()
    {
        controls = new InputsActionAsset();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MovementLogic();
        RotationLogic();
    }

    private void MovementLogic()
    {
        var moveDirection = controls.Player.Move.ReadValue<Vector2>();
        float currentSpeed = controls.Player.Sprint.IsInProgress() ? sprintSpeed : speed;

        rb.velocity = transform.rotation * new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.y * currentSpeed);       
    }

    private void RotationLogic()
    {
        var lookDirection = controls.Player.Look.ReadValue<Vector2>() * rotationSpeed;

        cameraRoot.localRotation *= Quaternion.AngleAxis(-lookDirection.y, Vector3.left);
        var cameraAngle = cameraRoot.localRotation.eulerAngles.x;
        cameraAngle = cameraAngle > 180 ?  cameraAngle - 360 : cameraAngle;
        cameraAngle = Mathf.Clamp(cameraAngle, -40, 40);
        cameraRoot.localRotation = Quaternion.Euler(cameraAngle, 0, 0);

        transform.localRotation *= Quaternion.AngleAxis(lookDirection.x, Vector3.up);
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
