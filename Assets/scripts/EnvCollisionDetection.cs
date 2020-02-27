using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvCollisionDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Finish")
        {
            Debug.Log("Finished.");
            other.gameObject.SetActive(false);
            StudyStateMachine.instance.MakeTransition();
        }
        DataLogger.instance.Collided();
        //Debug.Log("Boom!");
    }

    private void OnTriggerExit(Collider other)
    {
        DataLogger.instance.UnCollided();
        //Debug.Log("Out!");
    }
}
