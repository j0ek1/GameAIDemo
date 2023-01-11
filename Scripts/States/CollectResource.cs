using UnityEngine;

internal class CollectResource : IState
{
    private readonly Farmer farmer;

    public CollectResource(Farmer _farmer)
    {
        farmer = _farmer;
    }

    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        farmer.currentStateText = "Collect Resource";
        if (farmer.target != null)
        {
            farmer.TakeFromTarget();
            farmer.target = null;
        }
    }

    public void OnExit()
    {
        farmer.prevStateText = "Collect Resource";
    }
}