using UnityEngine;

public class TestMoveCode : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public Transform cameraTransform;

    [Header("Mouse Look")]
    public float mouseSensitivity = 150f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    // 누적 회전 값 (각도 저장용)
    private float yaw;   // 좌우 (Player)
    private float pitch; // 상하 (Camera)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 초기 회전값 동기화
        yaw = transform.eulerAngles.y;
        pitch = cameraTransform.localEulerAngles.x;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    // ▶ Quaternion 기반 시점 처리
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Player 좌우 회전
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Camera 상하 회전
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // ▶ 이동 처리 (카메라 기준)
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        Vector3 moveDir = (forward.normalized * v + right.normalized * h);

        transform.position += moveDir * speed * Time.deltaTime;
    }
}
