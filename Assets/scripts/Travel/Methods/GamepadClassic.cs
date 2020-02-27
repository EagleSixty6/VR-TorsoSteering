using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GamepadClassic : TravelBase
{
    [SerializeField] protected bool _strafingEnabled = false;

    protected override void Awake()
    {
        base.Awake();    
        _methodName = "GamePadClassic";
    }

    public override void Init()
    {
        base.Init();
        gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
    }

    protected override Vector3 GetDisiredMovementDirection()
    {
        Vector3 desiredMove = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * _input.y;

        if (_strafingEnabled)
        {
            desiredMove += Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized * _input.x;
        }
        return desiredMove;
    }

    protected override void RotateView()
    {
        float yRot = CrossPlatformInputManager.GetAxis("HorizontalRot") * XSensitivity;
        transform.RotateAround(Camera.main.transform.position, Vector3.up, yRot);  
    }

    public override void CleanUp()
    {
        gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
}

