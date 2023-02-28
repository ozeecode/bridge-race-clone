using Holex.StateMachine;

public class MoveState : IState
{
    private ContestantAI contestantAI;

    public MoveState(ContestantAI baseAI)
    {
        contestantAI = baseAI;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }


}
