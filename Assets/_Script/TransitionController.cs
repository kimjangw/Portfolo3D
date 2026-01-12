using System.Collections;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public Transform player;
    public CameraController cameraController;
    public TransitionHub hub;

    public enum Side { A, B }
    public Side portalSide;

    bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;
        if (hub.currentState != TransitionHub.State.None) return;

        if (portalSide == Side.A)
            hub.currentState = TransitionHub.State.FromA;
        else
            hub.currentState = TransitionHub.State.FromB;

        DoTransition();
    }

    void DoTransition()
    {
        isTransitioning = true;

        // 1) 월드 기준 X/Z 미러링
        Vector3 pos = player.position;
        pos.x = -pos.x;
        pos.z = -pos.z;
        player.position = pos;

        // 2) yaw 180 회전
        float newYaw = cameraController.yaw + 180f;
        cameraController.yaw = newYaw;
        player.rotation = Quaternion.Euler(0f, newYaw, 0f);

        // 3) 카메라 스냅 (yaw는 건드리지 않음)
        cameraController.SnapToPlayerInstant();

        // 4) 재트리거 방지용 offset
        player.position -= player.forward * 0.1f;

        StartCoroutine(ResetFlag());
    }




    IEnumerator ResetFlag()
    {
        yield return null;
        isTransitioning = false;
    }
}
