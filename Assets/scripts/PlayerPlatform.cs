using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    public static PlayerPlatform instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.GetChild(0).transform.position;
    }

    public Vector3 GetPlayerLocalPosition()
    {
        return transform.GetChild(0).transform.localPosition;
    }

    public Transform GetPlayer()
    {
        return transform.GetChild(0);
    }

    public Vector3 GetPlatformPosition()
    {
        return transform.position;
    }

    public Transform GetPlatform()
    {
        return transform;
    }
}
