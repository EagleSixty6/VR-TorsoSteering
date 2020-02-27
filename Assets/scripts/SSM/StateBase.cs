using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    private class StateTransition
    {
        public StateBase state;
        public string condition;

        public StateTransition(StateBase s, string c)
        {
            state = s;
            condition = c;
        }
    }

    private List<StateTransition> _successors;
    private StudyStateMachine _SSM;

    public StateBase()
    {
        _successors = new List<StateTransition>();
    }

    public virtual void OnEntry(){}

    public virtual void Update(){}

    public virtual void OnExit(){}

    public StateBase GetTransition(string condition)
    {
        // if no condition is given, just return the first transitions state
        if (condition == "" && _successors.Count > 0)
        {
            return _successors[0].state;
        }

        foreach(StateTransition transition in _successors)
        {
            if(condition == transition.condition)
            {
                return transition.state;
            }
        }

        return null;
    }

    public void AddSuccessor(StateBase state, string condition)
    {
        _successors.Add(new StateTransition(state, condition));
    }
}
