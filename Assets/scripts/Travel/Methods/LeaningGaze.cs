using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LeaningGaze : LeaningBase
{
    protected override void Awake()
    {
        base.Awake();
        _methodName = "LeaningGaze";
    }

    protected override Vector3 GetDisiredMovementDirection()
    {
        // move in gaze (HMD) direction
        return Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * _input.y;
    }
}

