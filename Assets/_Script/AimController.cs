using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    /* =========================================================
     * References
     * ========================================================= */
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform flashlight;      // 손전등 (회전만)
    [SerializeField] private Transform headAimTarget;   // Multi-Aim Target (빈 GO)

    /* =========================================================
     * Mouse Filter
     * ========================================================= */
    [Header("Mouse Speed Filter")]
    public float fastMouseThreshold = 600f;

    /* =========================================================
     * Wrist Pitch (Rotation Only)
     * ========================================================= */
    [Header("Wrist Pitch")]
    public float wristPitchMultiplier = 1f;
    public float wristMinPitch = -15f;
    public float wristMaxPitch = 25f;
    public float wristSmooth = 8f;

    /* =========================================================
     * Head Aim (Raycast)
     * ========================================================= */
    [Header("Head Aim (Range Based)")]
    public float headPitchMultiplier = 1f;   // 아주 작게
    public float headMinPitch = -5f;
    public float headMaxPitch = 8f;
    public float headSmooth = 6f;

    [Header("Head Aim Distance")]
    public float headAimDistance = 1.5f;
    /* =========================================================
     * Internal State
     * ========================================================= */
    Quaternion baseWristRotation;
    Vector3 baseHeadAimLocalPos;
    /* =========================================================
     * Unity Lifecycle
     * ========================================================= */
    void Start()
    {
        if (flashlight != null)
            baseWristRotation = flashlight.localRotation;

        if (headAimTarget != null)
            baseHeadAimLocalPos = headAimTarget.localPosition;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        if (mouseDelta.magnitude > fastMouseThreshold)
            return;

        float camPitch = cameraController.CurrentPitch;

        ApplyWristRotation(camPitch);
        ApplyHeadAimByRange(camPitch);
    }

    /* =========================================================
     * Wrist Control (Rotation Only)
     * ========================================================= */
    void ApplyWristRotation(float camPitch)
    {
        if (flashlight == null) return;

        float pitchOffset = camPitch * wristPitchMultiplier;
        pitchOffset = Mathf.Clamp(pitchOffset, wristMinPitch, wristMaxPitch);

        Quaternion targetRotation =
            baseWristRotation * Quaternion.Euler(-pitchOffset, 0f, 0f);

        flashlight.localRotation =
            Quaternion.Slerp(
                flashlight.localRotation,
                targetRotation,
                Time.deltaTime * wristSmooth
            );
    }

    /* =========================================================
     * Head Aim (Flashlight Raycast Target)
     * ========================================================= */

    void ApplyHeadAimByRange(float camPitch)
    {
        if (headAimTarget == null) return;

        // 앞쪽 기준
        Vector3 forwardOffset = Vector3.forward * headAimDistance;

        // 위/아래만 반응
        float headOffset = camPitch * headPitchMultiplier;

        Vector3 targetLocalPos =
            baseHeadAimLocalPos
            + forwardOffset
            + Vector3.up * (-headOffset * 0.01f);

        headAimTarget.localPosition = Vector3.Lerp(
            headAimTarget.localPosition,
            targetLocalPos,
            Time.deltaTime * headSmooth
        );
    }

}
