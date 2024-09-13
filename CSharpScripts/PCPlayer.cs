using UnityEngine;
using UnityEngine.InputSystem;
using IngameDebugConsole;

public class PCPlayer : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] float offset = 0.2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float checkGroundSize = 0.5f;
    static float sens = 0.15f;
    [Header("Interact")]
    [SerializeField] LayerMask generatorLayer;
    Transform itemPosition;
    Generator currentItem;
    CharacterController controller;
    Transform Orientation;
    Transform cam;
    Vector3 velocity;
    Vector2 move;
    Vector2 mouse;
    float yRotation = 0;
    bool isGrounded = true;
    bool jumpReady = true;
    bool isConsoleOut = false;
    private void Start()
    {
        sens = MainMenuManager.sensitivity;
        currentItem = null;
        itemPosition = transform.Find("Item Position");
        Orientation = transform.Find("Orientation");
        cam = Orientation.Find("Camera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        //if (Input.GetButtonDown("Cancel"))
        //{
        //    if (Cursor.lockState == CursorLockMode.Locked) {
        //        Cursor.lockState = CursorLockMode.Confined;
        //        Cursor.visible = true; 
        //    }
        //    else
        //    {
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //    } 
        //}

        Vector3 currentRotation = Orientation.rotation.eulerAngles;
        float normalizedXRotation = currentRotation.x > 180 ? currentRotation.x - 360 : currentRotation.x;
        mouse *= sens;
        transform.Rotate(0, mouse.x, 0);
        yRotation -= mouse.y;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
        if (yRotation < 40 && yRotation > -40)
        {
            Orientation.localRotation = Quaternion.Euler(yRotation, 0, 0);
        }
        else
        {
            cam.localRotation = Quaternion.Euler(yRotation - normalizedXRotation, 0, 0);
        }
        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * offset, checkGroundSize, groundMask);

        if (isGrounded && jumpReady)
        {
            controller.Move(Vector3.down);
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

        Vector3 dir = move.x*transform.right + move.y*transform.forward;
        controller.Move(speed * Time.deltaTime * dir.normalized);
        if (transform.position.y < -10)
        {
            transform.position = Vector3.up;
            velocity.y = 0;
        }
    }
    void Jump()
    {
        jumpReady = false;
        controller.Move(Vector3.up * (checkGroundSize/2+ 0.01f));
        velocity.y = Mathf.Sqrt(jumpForce * gravity * -2);
        Invoke("JumpReset", 0.5f);
    }
    void JumpReset()
    {
        jumpReady = true;
    }
    [ConsoleMethod("sens", "Set sensitivity. Default is 0.5")]
    public static void SetSensitivity(float _sens)
    {
        sens = _sens;
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && !isConsoleOut)
        {
            if (currentItem != null)
            {
                Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 100f, generatorLayer);
                if (hit.collider)
                {
                    currentItem.transform.LookAt(hit.point);
                }
                else
                {
                    currentItem.transform.LookAt(cam.forward * 100f);
                }
                currentItem.transform.Rotate(0, -90, 0);
                currentItem.Shoot(cam.forward,cam.position);
            }
            else
            {
                Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 4,generatorLayer);
                if (hit.rigidbody || hit.collider)
                {
                    hit.transform.parent = itemPosition;
                    hit.transform.localPosition = Vector3.zero;
                    currentItem = hit.transform.GetComponent<Generator>();
                }
            }
        }
    }
    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && !isConsoleOut)
        {
            currentItem.transform.parent = null;
            currentItem = null;
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (isConsoleOut) return;
        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * offset, checkGroundSize, groundMask);
        if (context.performed && isGrounded && jumpReady)
        {
            Jump();
        }
    }
    public void MovementPlayer(InputAction.CallbackContext context)
    {
        if (!isConsoleOut) move = context.ReadValue<Vector2>();
    }
    public void MouseMove(InputAction.CallbackContext context)
    {
        if (!isConsoleOut) mouse = context.ReadValue<Vector2>();
    }
    public void Console(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isConsoleOut)
            {
                isConsoleOut = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else
            {
                isConsoleOut = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StuckDoor"))
        {
            controller.Move(transform.forward * -3);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (1 + offset));
        Gizmos.DrawSphere(transform.position + Vector3.up * offset, checkGroundSize);
    }
}
