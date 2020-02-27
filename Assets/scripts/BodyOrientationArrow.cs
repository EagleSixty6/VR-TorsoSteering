using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyOrientationArrow : MonoBehaviour
{
    void Update()
    {
        transform.rotation = transform.parent.transform.parent.transform.rotation * Quaternion.Euler(0, 90, 90);
    }
}