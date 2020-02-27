using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LeaningBase : TravelBase
{
    [SerializeField] public GameObject _torsoTracker;
    [Range(0f, 1f)] [SerializeField] private float _leaningDeadzone;
    protected GameObject _leaningKS;


    public override void Init()
    {
        base.Init();
        _leaningKS = new GameObject("Leaning");
        _leaningKS.transform.parent = _torsoTracker.transform;
        _leaningKS.transform.localPosition = Vector3.zero;
        _leaningKS.transform.localRotation = Quaternion.Inverse(_torsoTracker.transform.rotation);
        _leaningKS.transform.rotation = Quaternion.Euler(_leaningKS.transform.rotation.eulerAngles.x, _torsoTracker.transform.rotation.eulerAngles.y, _leaningKS.transform.rotation.eulerAngles.z);
    }

    protected override void GetInput()
    {
        // Read input
        float horizontal = 0; // no strafing
        float vertical = _leaningKS.transform.rotation.eulerAngles.x;

        if (vertical > 180)
        {
            vertical -= 360;
        }
        
        vertical = vertical / 20;
        if (vertical < 0 && vertical > -_leaningDeadzone || vertical > 0 && vertical < _leaningDeadzone)
        {
            vertical = 0;
        }
        _input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (_input.sqrMagnitude > 1)
        {
            _input.Normalize();
        }
    }
}

