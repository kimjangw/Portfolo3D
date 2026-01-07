using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //기준이될 플레이어 Transform
    public Transform player;

    //카메라 측에서 보는 값을 다른 스크립트로 전달.
    public CameraView CurrentView { get; private set; }
    public struct CameraView
    {
        public float pitch;
        public float yaw;
        public Vector3 forward;
        public Vector3 right;
        public Vector3 up;
    }
    

    //TPS구현을 위한 카메라의 등뒤 위치 세팅.
    [Header("CameraOffset")]
    public float cameraBackDistance = 1.1f;        // 뒤
    public float cameraHeight = 1.12f;          // 위
    public float cameraShoulderOffset = 0.2f;  // 우측 어깨

    //민감도 조절 및 최대 시야각(위,아래)조절
    [Header("CameraRotation")]
    public float sensitivity = 20f;
    public float minPitch = -25f;
    public float maxPitch = 25f;

    //플레이어 좌,우 회전 변수
    float yaw;
    //카메라 위, 아래 각도 변수
    float pitch = 10f;

    void Start()
    {
        //마우스 중앙 고정 및 감추기.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //플레이어 최초 Y값 세팅
        yaw = player.eulerAngles.y;
    }
    
    //카메라 흔들림 제어를 위해 update다음 실행되는 LateUpde 이용
    void LateUpdate()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue();

        // 마우스.X좌표 이동 → 캐릭터 Y축 회전
        yaw += mouse.x * sensitivity * Time.deltaTime;
        player.rotation = Quaternion.Euler(0f, yaw, 0f);

        // 마우스.Y좌표 이동 → 카메라 Pitch(상하) 회전
        pitch -= mouse.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 카메라 각도 계산
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);

        // 카메라 등뒤 위치 계산.
        Vector3 offset =
            Vector3.up * cameraHeight +
            Vector3.back * cameraBackDistance +
            Vector3.right * cameraShoulderOffset;

        //카메라 최종 위치 세팅.
        transform.position = player.position + cameraRotation * offset;
        transform.rotation = cameraRotation;

        //다른 스크립트로 보낼 변수 초기화.
        CurrentView = new CameraView
        {
            pitch = pitch,
            yaw = yaw,
            forward = transform.forward,
            right = transform.right,
            up = transform.up
        };

    }
}
