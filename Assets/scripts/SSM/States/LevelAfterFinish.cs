using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Valve.VR;

public class LevelAfterFinish : StateBase
{
    private GameObject _dialog = null;
    private GameObject _arrow = null;

    public override void OnEntry()
    {
        _dialog = GameObject.Find("Level").transform.Find("PointToStart").gameObject;
        _dialog.SetActive(true);
        _arrow = PlayerPlatform.instance.GetPlayer().Find("TorsoArrow").gameObject;
        _arrow.SetActive(true);
    }

    public override void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            StudyStateMachine.instance.MakeTransition();
        }
    }

    public override void OnExit()
    {
        Vector3 lineToStart = DataLogger.instance.GetPlayerStartingPositon() - PlayerPlatform.instance.GetPlayerPosition();
        Vector3 lineOfSight = PlayerPlatform.instance.GetPlayer().forward;
        float angleError = Vector3.SignedAngle(lineToStart, lineOfSight, Vector3.up);
        Debug.Log("Looking error: " + angleError);
        DataLogger.instance.WriteOrientationErrorLog(angleError);

        _dialog.SetActive(false);
        _arrow.SetActive(false);
    }
}