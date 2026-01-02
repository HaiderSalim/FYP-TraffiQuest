using UnityEngine;

public class FreeMovementCamera : MonoBehaviour
{
    [Header("Camera Parameters")]
    [SerializeField] private Vector2 Turn;
    [SerializeField, Range(0.1f, 5f)] private float Sensitivity = 2f;
    [SerializeField, Range(0.1f, 10f)] private float camSpeed = 0.5f;

    // Input System
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    float Horizontalinput;
    float Verticalinput;
    
    private Camera Cam;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        Cam = GetComponent<Camera>();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Look
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void Update()
    {
        SetLookAxis();
        MoveCamera();
        MovePlayer();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void SetLookAxis()
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        Turn.x += lookInput.x * Sensitivity * Time.deltaTime;
        Turn.y += lookInput.y * Sensitivity * Time.deltaTime;

        Turn.y = Mathf.Clamp(Turn.y, -50f, 30f);

        Horizontalinput = horizontal;
        Verticalinput = vertical;
    }

    void MoveCamera()
    {
        Vector3 val = Vector3.up * Turn.x;//This is for left and right camers movement.
        Cam.transform.localRotation = Quaternion.Euler(-Turn.y, val.y, 0);//This is for up and down camera movement
    }

    void MovePlayer()
    {
        if (Horizontalinput != 0)
        {
            transform.Translate(transform.right * Horizontalinput * Time.deltaTime * camSpeed, Space.World);
            // rb.AddForce(playerSpeed * Acceleration * Horizontalinput * Cam.transform.right, ForceMode.VelocityChange);
        }
        if (Verticalinput != 0)
        {
            transform.Translate(transform.forward * Verticalinput * Time.deltaTime * camSpeed, Space.World);
            // rb.AddForce(playerSpeed * Acceleration * Verticalinput * Ancor.forward, ForceMode.VelocityChange);
        }
    }

    public void GetHorizontalInput(float horizontal)
    {
        Horizontalinput = horizontal;
        MovePlayer();
    }
    public void GetVerticalInput(float vertical)
    {
        Verticalinput = vertical;
        MovePlayer();
    }
}
