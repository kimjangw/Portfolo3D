using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;

    [Header("Offset")]
    public float distance = 1.1f;        // 뒤
    public float height = 1.12f;          // 위
    public float shoulderOffset = 0.2f;  // 우측 어깨

    [Header("Rotation")]
    public float sensitivity = 20f;
    public float minPitch = -25f;
    public float maxPitch = 25f;

    float yaw;
    float pitch = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = player.eulerAngles.y;
    }

    void LateUpdate()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue();

        // 1️ 마우스 → 캐릭터 Y 회전
        yaw += mouse.x * sensitivity * Time.deltaTime;
        player.rotation = Quaternion.Euler(0f, yaw, 0f);

        // 2️ 카메라 Pitch (카메라만)
        pitch -= mouse.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 3️ 카메라 위치 계산 (등 뒤 + 위 + 우측 어깨)
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset =
            Vector3.up * height +
            Vector3.back * distance +
            Vector3.right * shoulderOffset;

        transform.position = player.position + camRot * offset;
        transform.rotation = camRot;
    }
}
