using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GamepadTorso : TravelBase
{
    [SerializeField] public GameObject _torsoTracker;
    protected GameObject _leaningKS;

    protected override void Awake()
    {
        base.Awake();
        _methodName = "GamePadTorso";
    }

    public override void Init()
    {
        base.Init();
        _leaningKS = new GameObject("Leaning");
        _leaningKS.transform.parent = _torsoTracker.transform;
        _leaningKS.transform.localPosition = Vector3.zero;
        _leaningKS.transform.localRotation = Quaternion.Inverse(_torsoTracker.transform.rotation);
        _leaningKS.transform.rotation = Quaternion.Euler(_leaningKS.transform.rotation.eulerAngles.x, _torsoTracker.transform.rotation.eulerAngles.y, _leaningKS.transform.rotation.eulerAngles.z);
    }

    protected override Vector3 GetDisiredMovementDirection()
    {
        // move in Torso tracker direction
        return Vector3.ProjectOnPlane(_leaningKS.transform.forward, Vector3.up).normalized * _input.y;
    }
}

