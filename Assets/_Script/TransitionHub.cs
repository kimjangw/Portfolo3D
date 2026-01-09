using UnityEngine;

public class TransitionHub : MonoBehaviour
{
    public enum State { None, FromA, FromB }
    public State currentState = State.None;

    // Lock: 반대 방향 재진입 방지 용도
    public bool A_locked = false;
    public bool B_locked = false;

    public enum StartSide { A, B }
    public StartSide startSide = StartSide.A;

    void Start()
    {
        // 시작 초기 Lock 상태 설정
        if (startSide == StartSide.A)
        {
            // A에서 출발 → B로 바로 되돌아가는 것을 막는다
            A_locked = false;
            B_locked = true;
        }
        else
        {
            A_locked = true;
            B_locked = false;
        }

        currentState = State.None;
    }

    public void LockA() => A_locked = true;
    public void LockB() => B_locked = true;

    public void UnlockAll()
    {
        A_locked = false;
        B_locked = false;
        currentState = State.None;
    }

    // LineCheck에서 처리
    private void OnTriggerEnter(Collider other)
    {
        UnlockAll();
    }
}
