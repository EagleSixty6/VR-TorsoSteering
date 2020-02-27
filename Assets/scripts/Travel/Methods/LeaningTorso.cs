using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LeaningTorso : LeaningBase
{
    protected override void Awake()
    {
        base.Awake();
        _methodName = "LeaningTorso";
    }

    protected override Vector3 GetDisiredMovementDirection()
    {
        // move in Torso tracker direction
        return Vector3.ProjectOnPlane(_leaningKS.transform.forward, Vector3.up).normalized * _input.y;
    }
}

