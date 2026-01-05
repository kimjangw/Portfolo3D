using UnityEngine;

public class TestSpotLight : MonoBehaviour
{
     [Header("Target")]
    [Tooltip("플레이어 카메라 Transform")]
    public Transform cameraTransform;

    [Header("Position Offset")]
    public Vector3 localPositionOffset = new Vector3(0f, 0f,2f);

    [Header("Rotation Offset")]
    public Vector3 localRotationOffset = Vector3.zero;

    [Header("Option")]
    public bool followRotation = true;
    public bool followPosition = true;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 위치 연동
        if (followPosition)
        {
            transform.position =
                cameraTransform.TransformPoint(localPositionOffset);
        }

        // 회전 연동 (Quaternion)
        if (followRotation)
        {
            Quaternion targetRot =
                cameraTransform.rotation * Quaternion.Euler(localRotationOffset);

            transform.rotation = targetRot;
        }
    }
}
