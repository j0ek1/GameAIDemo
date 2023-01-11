using System;
using UnityEngine;
using UnityEngine.AI;

public class Farmer : MonoBehaviour
{  
    [SerializeField] private float maxCollection = 4000;
    private StateMachine stateMachine;
    public AStar pathfind;
    public GridA grid;
    public float collected = 0;
    public float speed;
    private float t = 1;
    private int pathIndex = 0;
    public Generator target;
    public Store store;
    public GameObject enemy;
    public string currentStateText;
    public string prevStateText;
    public bool stopped = false;

    private SearchForResource search;
    private MoveToResource moveToResource;
    private CollectResource collect;
    private MoveToStore moveToStore;
    private StoreResource storeResource;
    private Stop stop;
    private Flee flee;

    private void Awake()
    {
        currentStateText = null;
        prevStateText = null;
        
        stateMachine = new StateMachine();

        // Possible states farmer can be in
        search = new SearchForResource(this, pathfind, grid);
        moveToResource = new MoveToResource(this, pathfind, grid);
        collect = new CollectResource(this);
        moveToStore = new MoveToStore(this, pathfind, grid);
        storeResource = new StoreResource(this);
        stop = new Stop(this);
        flee = new Flee(this, pathfind, grid, enemy);

        // Adding possible transitions
        AddT(flee, search, Stuck());
        AddT(storeResource, stop, StoreFull());
        AddT(search, moveToStore, ShouldStore());
        AddT(search, moveToResource, HasTarget());
        AddT(moveToResource, collect, ReachedResource());
        AddT(collect, search, CanGetMore());   
        AddT(collect, moveToStore, InventoryFull());   
        AddT(moveToStore, storeResource, ReachedStore());
        AddT(storeResource, search, () => collected == 0);
        AddT(flee, search, EnemyFar());
        stateMachine.AddAnyTransition(flee, EnemyClose());       

        // First state called on awake
        stateMachine.SetState(search);

        void AddT(IState to, IState from, Func<bool> condition)
        {
            stateMachine.AddTransition(to, from, condition);
        }

        // Func<bool> are used to determine whether conditions have been met
        Func<bool> Stuck()
        {
            return () => flee.timeStuck > 1f;
        }
        Func<bool> HasTarget()
        {
            return () => target != null;
        }
        Func<bool> EnemyClose()
        {
            return () => Vector3.Distance(transform.position, enemy.transform.position) < 15f;
        }
        Func<bool> EnemyFar()
        {
            return () => Vector3.Distance(transform.position, enemy.transform.position) > 30f;
        }
        Func<bool> ReachedResource()
        {
            return () => target != null && Vector3.Distance(transform.position, target.transform.position) < 2.5f;
        }
        Func<bool> CanGetMore()
        {
            return () => (target == null) && !InventoryFull().Invoke();
        }
        Func<bool> InventoryFull()
        {
            return () => collected >= maxCollection;
        }
        Func<bool> ShouldStore()
        {
            // Desirability of storing instead of collecting more resources
            return () => target != null && (((collected / maxCollection) - .1f) > search.genDesire[search.bgIndex]);
        }
        Func<bool> ReachedStore()
        {
            return () => store != null && Vector3.Distance(transform.position, store.transform.position) < 3f;
        }
        Func<bool> StoreFull()
        {
            return () => store != null && store.currentStorage >= store.maxStorage;
        }
    }

    private void Update() // Call state machine
    {
        stateMachine.Tick();

        t = Time.deltaTime * speed;
        if (grid.path != null)
        {
            // If farmer is not at the current node
            if (grid.NodeFromWorldPoint(transform.position) != grid.path[pathIndex])
            {
                // Move towards node
                transform.position = Vector3.MoveTowards(transform.position, (grid.path[pathIndex].worldPos + Vector3.up), t);
            }
            else // If farmer is at current node, move to next node
            {
                if (pathIndex < grid.path.Count-1)
                {
                    pathIndex++;
                }
                else // If we have reached the end node reset
                {
                    pathIndex = 0;
                    grid.path = null;
                }
            }
        }
        else
        {
            pathIndex = 0;
        }
    }

    // Actions
    public void TakeFromTarget()
    {
        if (target.CanTake())
        {
            collected += target.taken;
            if (collected > maxCollection)
            {
                collected = maxCollection;
            }
        }
    }
    public void StoreResources()
    {
        if (store.Give())
        {
            store.currentStorage += collected;
            collected = 0;
        }
    }
    public void DropAll()
    {
        collected = 0;
    }
}