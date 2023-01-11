using UnityEngine;

internal class Stop : IState
{
    private readonly Farmer farmer;

    public Stop(Farmer _farmer)
    {
        farmer = _farmer;
    }

    public void Tick()
    {

    }

    public void OnEnter()
    {
        farmer.currentStateText = "Stop";
        farmer.stopped = true;
    }

    public void OnExit()
    {
        farmer.prevStateText = "Stop";
    }
}