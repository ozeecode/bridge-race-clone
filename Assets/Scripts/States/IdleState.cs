using Holex.StateMachine;


public class IdleState : IState
{
    ContestantAI contestantAI;

    public IdleState(ContestantAI baseAI)
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
