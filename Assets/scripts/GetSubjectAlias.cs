using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetSubjectAlias : MonoBehaviour
{
    public void GetAndSetAlias()
    {
        SessionManager.instance.SetSubjectAlias(transform.GetComponentInChildren<InputField>().text);
    }
}
