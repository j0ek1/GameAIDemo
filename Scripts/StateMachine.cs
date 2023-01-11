using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class StateMachine
{
    public IState currentState;

    // Transition variables
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();  
    private static List<Transition> emptyTransitions = new List<Transition>(0);

    public void Tick()
    {
        var transition = GetTransition(); // Look for transition

        // If we get transition, set state
        if (transition != null)
        {
            SetState(transition.To);
        }

        currentState?.Tick(); // Tick current state (if not null)
    }

    // Transitioning to the next state
    public void SetState(IState state)
    {
        if (state == currentState) // If the state is the same, return
        {
            return;
        }

        currentState?.OnExit(); // Call exit function of previous state (if not null)
        currentState = state; // Assign next state

        _transitions.TryGetValue(currentState.GetType(), out currentTransitions); // Try get list of transitions for this type
        if (currentTransitions == null) // If null, set list to empty
        {
            currentTransitions = emptyTransitions;
        }

        currentState?.OnEnter(); // Call enter function of next state
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        // Try get list of transitions from state, if false create new list
        if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition // Create transition class
    {
        public Func<bool> Condition { get; }
        public IState To { get; } // State we are going to transition to

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        // Check transitions which can transition from anything first
        foreach(var transition in anyTransitions)
        {
            if (transition.Condition()) // Check if conditions return true
            {
                return transition; // THIS IS WHERE THE ORDER OF TRANSITIONS MATTERS, NEED TO CHANGE DEPENDING ON DESIRE
            }
        }

        // Then check current state transitions
        foreach (var transition in currentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }
}