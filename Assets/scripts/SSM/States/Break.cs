using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Break : StateBase
{
    public override void OnEntry()
    {
        SessionManager.instance.SpawnBreakDialog();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StudyStateMachine.instance.MakeTransition();
        }
    }

}
