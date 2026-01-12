using UnityEngine;

public class TransitionHub : MonoBehaviour
{
    public enum State { None, FromA, FromB }
    public State currentState = State.None;

    public enum StartSide { A, B }
    public StartSide startSide = StartSide.A;

    void Start()
    {
        currentState = State.None;
    }

    public void ResetState()
    {
        currentState = State.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        ResetState();
    }
}
