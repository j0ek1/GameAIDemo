using UnityEngine;

internal class StoreResource : IState
{
    private readonly Farmer farmer;

    public StoreResource(Farmer _farmer)
    {
        farmer = _farmer;
    }

    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        farmer.currentStateText = "Store Resource";
        farmer.StoreResources();
        farmer.target = null;
    }

    public void OnExit()
    {
        farmer.prevStateText = "Store Resource";
    }
}