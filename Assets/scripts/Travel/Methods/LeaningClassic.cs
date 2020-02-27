using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LeaningClassic : LeaningBase
{
    [Range(0f, 90f)][SerializeField] private float _scrollingDeadzone;

    protected override void Awake()
    {
        base.Awake();
        _methodName = "LeaningClassic";
    }

    public override void Init()
    {
        base.Init();
        gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
    }


    protected override Vector3 GetDisiredMovementDirection()
    {
        return Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * _input.y;
    }

    protected override void RotateView()
    {
        Vector3 playerForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        float lookingAngleDiff = Vector3.SignedAngle(playerForward, cameraForward, Vector3.up);
        float processedlookingAngle = Mathf.Abs(lookingAngleDiff) - _scrollingDeadzone;
       
        float val = 0f;
        if(processedlookingAngle > 0)
        {
            val = processedlookingAngle / (90f - _scrollingDeadzone);
        }
        
        float yRot = val * XSensitivity;
        transform.RotateAround(Camera.main.transform.position, Vector3.up, yRot * Mathf.Sign(lookingAngleDiff));
    }

    public override void CleanUp()
    {
        gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
}

