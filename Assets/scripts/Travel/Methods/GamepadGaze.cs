using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GamepadGaze : TravelBase
{
    [SerializeField] protected bool _strafingEnabled = false;
    [SerializeField] protected bool _virtualRotEnabled = false;

    protected override void Awake()
    {
        base.Awake();
        _methodName = "GamePadGaze";
    }

    protected override Vector3 GetDisiredMovementDirection()
    {
        Vector3 desiredMove = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * _input.y;

        if(_strafingEnabled)
        {
            desiredMove += Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * _input.x;
        }
       return desiredMove;
    }

    protected override void RotateView()
    {
        if(_virtualRotEnabled)
        {
            float yRot = CrossPlatformInputManager.GetAxis("HorizontalRot") * XSensitivity;
            transform.RotateAround(Camera.main.transform.position, Vector3.up, yRot);
        }       
    }
}

