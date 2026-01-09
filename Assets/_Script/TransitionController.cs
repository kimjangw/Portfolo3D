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

        // 기준축을 월드 원점 기준으로 미러 (z + x)
        Vector3 pos = player.position;
        pos = new Vector3(-pos.x, pos.y, -pos.z);
        player.position = pos;

        // 회전 180
        float newYaw = cameraController.yaw + 180f;
        cameraController.yaw = newYaw;
        player.rotation = Quaternion.Euler(0f, newYaw, 0f);

        // 카메라 스냅
        cameraController.SnapToPlayerInstant();

        // 재트리거 방지
        player.position -= player.forward * 0.1f;

        StartCoroutine(ResetFlag());
    }



    IEnumerator ResetFlag()
    {
        yield return null;
        isTransitioning = false;
    }
}
