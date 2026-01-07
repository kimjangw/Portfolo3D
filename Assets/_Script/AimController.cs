using UnityEngine;

public class AimController : MonoBehaviour
{
    /* =========================================================
     * References
     * ========================================================= */
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform flashlight;      // 손전등 (회전만)
    [SerializeField] private Transform headAimTarget;   // Head Multi-Aim Target

    /* =========================================================
     * Wrist (Flashlight)
     * ========================================================= */
    [Header("Wrist Aim")]
    public float wristPitchMultiplier = 1f;
    public float wristMinPitch = -15f;
    public float wristMaxPitch = 25f;
    public float wristSmooth = 8f;

    /* =========================================================
     * Head Aim
     * ========================================================= */
    [Header("Head Aim")]
    public float headPitchMultiplier = 1f;
    public float headSmooth = 6f;
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

    void LateUpdate()
    {
        if (cameraController == null) return;

        float camPitch = cameraController.CurrentView.pitch;

        ApplyWristAim(camPitch);
        ApplyHeadAim(camPitch);
    }

    /* =========================================================
     * Wrist Aim (Rotation Only)
     * ========================================================= */
    void ApplyWristAim(float camPitch)
    {
        if (flashlight == null) return;

        float pitchOffset = Mathf.Clamp(
            camPitch * wristPitchMultiplier,
            wristMinPitch,
            wristMaxPitch
        );

        Quaternion targetRotation =
            baseWristRotation * Quaternion.Euler(-pitchOffset, 0f, 0f);

        flashlight.localRotation = Quaternion.Slerp(
            flashlight.localRotation,
            targetRotation,
            Time.deltaTime * wristSmooth
        );
    }

    /* =========================================================
     * Head Aim (Range Based)
     * ========================================================= */
    void ApplyHeadAim(float camPitch)
    {
        if (headAimTarget == null) return;

        Vector3 targetLocalPos =
            baseHeadAimLocalPos
            + Vector3.forward * headAimDistance
            + Vector3.up * (-camPitch * headPitchMultiplier * 0.01f);

        headAimTarget.localPosition = Vector3.Lerp(
            headAimTarget.localPosition,
            targetLocalPos,
            Time.deltaTime * headSmooth
        );
    }
}
