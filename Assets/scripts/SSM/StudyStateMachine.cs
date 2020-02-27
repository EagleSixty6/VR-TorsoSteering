using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyStateMachine : MonoBehaviour
{
    private StateBase _currentState = null;
    public static StudyStateMachine instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InitAndStart(StateBase initialState)
    {
        _currentState = initialState;
        _currentState.OnEntry();
    }

    void Update()
    {
        if(_currentState != null)
        {
            _currentState.Update();
        }
    }

    public void MakeTransition()
    {
        MakeTransition("");
    }

    public void MakeTransition(string condition)
    {
        _currentState.OnExit();
        _currentState = _currentState.GetTransition(condition);
        _currentState.OnEntry();             
    }
}
