using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoTracker : MonoBehaviour
{
    private static TorsoTracker instance = null;

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
}
